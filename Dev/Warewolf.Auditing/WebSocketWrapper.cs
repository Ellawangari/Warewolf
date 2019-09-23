﻿/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Net.WebSockets.Managed;
namespace Warewolf.Auditing
{
    public interface IWebSocketFactory
    {
        IWebSocketWrapper New();
    }

    public class WebSocketFactory : IWebSocketFactory
    {
        public IWebSocketWrapper New()
        {
            return WebSocketWrapper.Create("ws://localhost:5000/ws");
        }
    }

    public interface IWebSocketWrapper
    {
        WebSocketWrapper Connect();
        WebSocketWrapper OnConnect(Action<WebSocketWrapper> onConnect);
        WebSocketWrapper OnDisconnect(Action<WebSocketWrapper> onDisconnect);
        WebSocketWrapper OnMessage(Action<string, WebSocketWrapper> onMessage);
        void SendMessage(string message);
    }

    public class WebSocketWrapper : IWebSocketWrapper
    {
        private const int ReceiveChunkSize = 1024;
        private const int SendChunkSize = 1024;

        private readonly System.Net.WebSockets.Managed.ClientWebSocket _ws;
        private readonly Uri _uri;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly CancellationToken _cancellationToken;

        private Action<WebSocketWrapper> _onConnected;
        private Action<string, WebSocketWrapper> _onMessage;
        private Action<WebSocketWrapper> _onDisconnected;

        protected WebSocketWrapper(string uri)
        {
            _ws = new System.Net.WebSockets.Managed.ClientWebSocket();
            _ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(20);
            _uri = new Uri(uri);
            _cancellationToken = _cancellationTokenSource.Token;
        }

        public static WebSocketWrapper Create(string uri)
        {
            return new WebSocketWrapper(uri);
        }

        public WebSocketWrapper Connect()
        {
            ConnectAsync();
            return this;
        }

        public WebSocketWrapper OnConnect(Action<WebSocketWrapper> onConnect)
        {
            _onConnected = onConnect;
            return this;
        }

        public WebSocketWrapper Close()
        {
            _ws.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", _cancellationToken);
            return this;
        }

        public WebSocketWrapper OnDisconnect(Action<WebSocketWrapper> onDisconnect)
        {
            _onDisconnected = onDisconnect;
            return this;
        }

        public WebSocketWrapper OnMessage(Action<string, WebSocketWrapper> onMessage)
        {
            _onMessage = onMessage;
            return this;
        }

        public void SendMessage(string message)
        {
            SendMessageAsync(message);
        }

        private async void SendMessageAsync(string message)
        {
            if (_ws.State != WebSocketState.Open)
            {
                throw new Exception("Connection is not open.");
            }

            var messageBuffer = Encoding.UTF8.GetBytes(message);
            var messagesCount = (int)Math.Ceiling((double)messageBuffer.Length / SendChunkSize);

            for (var i = 0; i < messagesCount; i++)
            {
                var offset = (SendChunkSize * i);
                var count = SendChunkSize;
                var lastMessage = ((i + 1) == messagesCount);

                if ((count * (i + 1)) > messageBuffer.Length)
                {
                    count = messageBuffer.Length - offset;
                }

                await _ws.SendAsync(new ArraySegment<byte>(messageBuffer, offset, count), WebSocketMessageType.Text, lastMessage, _cancellationToken);
            }
        }

        private async void ConnectAsync()
        {
            await _ws.ConnectAsync(_uri, _cancellationToken);
            CallOnConnected();
            StartListen();
        }

        private async void StartListen()
        {
            var buffer = new byte[ReceiveChunkSize];

            try
            {
                while (_ws.State == WebSocketState.Open)
                {
                    var stringResult = new StringBuilder();


                    WebSocketReceiveResult result;
                    do
                    {
                        result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationToken);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await
                                _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                            CallOnDisconnected();
                        }
                        else
                        {
                            var str = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            stringResult.Append(str);
                        }

                    } while (!result.EndOfMessage);

                    CallOnMessage(stringResult);

                }
            }
            catch (Exception)
            {
                CallOnDisconnected();
            }
            finally
            {
                _ws.Dispose();
            }
        }

        private void CallOnMessage(StringBuilder stringResult)
        {
            if (_onMessage != null)
            {
                RunInTask(() => _onMessage(stringResult.ToString(), this));
            }
        }

        private void CallOnDisconnected()
        {
            if (_onDisconnected != null)
            {
                RunInTask(() => _onDisconnected(this));
            }
        }

        private void CallOnConnected()
        {
            if (_onConnected != null)
            {
                RunInTask(() => _onConnected(this));
            }
        }

        private static void RunInTask(Action action)
        {
            Task.Factory.StartNew(action);
        }
    }
}