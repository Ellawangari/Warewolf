/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2017 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using Dev2.Common.Interfaces.StringTokenizer.Interfaces;
using System;
using System.Text;
using Warewolf.Resource.Errors;



namespace Dev2.Common
{
    internal class Dev2TokenOp : IDev2SplitOp
    {
        readonly string _escapeChar;
        readonly bool _include;
        readonly char[] _tokenParts;

        internal Dev2TokenOp(string token, bool includeToken)
            : this(token, includeToken, "")
        {
        }

        internal Dev2TokenOp(string token, bool includeToken, string escape)
        {
            _include = includeToken;
            _tokenParts = token.ToCharArray();
            _escapeChar = escape;
        }

        public bool IsFinalOp()
        {
            return false;
        }

        public bool CanUseEnumerator(bool isReversed)
        {
            return !isReversed && _tokenParts.Length == 1;
        }

        public string ExecuteOperation(char[] candidate, int startIdx, bool isReversed)
        {
            var result = new StringBuilder();

            if (!isReversed)
            {                
                if (_tokenParts.Length == 1)
                {
                    int pos = startIdx;
                    while (pos < candidate.Length &&
                           (candidate[pos] != _tokenParts[0] || SkipDueToEscapeChar(candidate, pos)))
                    {
                        result.Append(candidate[pos]);
                        pos++;
                    }
                }
                else
                {
                    int pos = startIdx;                   
                    while (pos < candidate.Length && !IsMultiTokenMatch(candidate, pos, false))
                    {                        
                        result.Append(candidate[pos]);
                        pos++;
                    }
                }

                // did the user want the token included?
                if (_include && result.Length + startIdx < candidate.Length)
                {
                    result.Append(_tokenParts);
                }
            }
            else
            {
                // reverse order
                if (_tokenParts.Length == 1)
                {
                    int pos = startIdx;
                    if (pos > candidate.Length)
                    {
                        pos = candidate.Length - 1;
                    }
                    while (pos >= 0 && (candidate[pos] != _tokenParts[0] || SkipDueToEscapeChar(candidate, pos)))
                    {
                        result.Insert(0, candidate[pos]);
                        pos--;
                    }
                }
                else
                {
                    int pos = startIdx;
                    while (pos >= 0 && !IsMultiTokenMatch(candidate, pos, true))
                    {
                        result.Insert(0, candidate[pos]);
                        pos--;
                    }
                }

                if (_include && startIdx - result.Length >= 0)
                {
                    result.Insert(0, _tokenParts);
                }
            }

            return result.ToString();
        }

        public string ExecuteOperation(CharEnumerator canidate, int startIdx, int len, bool isReversed)
        {
            var result = new StringBuilder();

            if (!isReversed && _tokenParts.Length == 1)
            {
                if (_tokenParts.Length == 1)
                {
                    char tmp;

                    // fetch next value while
                    while (canidate.MoveNext() &&
                           ((tmp = canidate.Current) != _tokenParts[0] || SkipDueToEscapeChar(result.ToString())))
                    {
                        result.Append(tmp);
                    }
                }

                // did they want the token included?
                if (_include && startIdx + result.Length < len)
                {
                    result.Append(_tokenParts);
                }
            }
            else
            {
                throw new Exception(ErrorResource.CharEnumeratorNotSupported);
            }

            return result.ToString();
        }

        public int OpLength()
        {
            int result = _tokenParts.Length;

            if (_include)
            {
                result -= _tokenParts.Length;
            }

            return result;
        }

        #region Private Method

        bool IsMultiTokenMatch(char[] canidate, int fromIndex, bool isReversed)
        {
            bool result = true;

            int cnt = 0;
            int canidateIdx = fromIndex;

            if (isReversed)
            {
                cnt = _tokenParts.Length - 1;
            }

            if (canidateIdx - (_tokenParts.Length - 1) >= 0 && isReversed || !isReversed)
            {
                while (cnt < _tokenParts.Length && cnt >= 0 && canidateIdx >= 0 && canidateIdx < canidate.Length && result)
                {
                    if (canidate[canidateIdx] != _tokenParts[cnt])
                    {
                        result = false;
                    }
                    if (!isReversed)
                    {
                        canidateIdx++;
                        cnt++;
                    }
                    else
                    {
                        canidateIdx--;
                        cnt--;
                    }
                }
            }
            else
            {
                result = false; // no way we can match, not enough chars ;)
            }

            return result;
        }

        #endregion Private Method

        bool SkipDueToEscapeChar(char[] candidate, int pos)
        {
            if (pos > 0 && !String.IsNullOrEmpty(_escapeChar))
            {
                return candidate[pos - 1] == _escapeChar[0];
            }
            return false;
        }

        bool SkipDueToEscapeChar(string word)
        {
            if (!String.IsNullOrEmpty(_escapeChar))
            {
                return word.Contains(_escapeChar) && word.EndsWith(_escapeChar);

            }
            return false;
        }
    }
}