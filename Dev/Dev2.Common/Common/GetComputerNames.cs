/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2017 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Management;
using System.Security.Principal;


namespace Dev2.Common.Common
{
    public class GetComputerNames
    {
        private static List<string> _currentComputerNames;
        
        public static List<string> ComputerNames
        {
            get
            {
                if (_currentComputerNames == null)
                {
                    GetComputerNamesList();
                }

                return _currentComputerNames;
            }
        }

        public static void GetComputerNamesList()
        {
            _currentComputerNames = StandardComputerNameQuery();
        }
        
        private static List<string> StandardComputerNameQuery()
        {
            WindowsIdentity wi = WindowsIdentity.GetCurrent();

            if (wi != null)
            {
                string serverUserName = wi.Name;

                string[] parts = serverUserName.Split('\\');

                string queryStr = "WinNT://";
                
                if (parts.Length == 2)
                {
                    queryStr += parts[0];
                }
                else
                {
                    var query = new SelectQuery("Win32_ComputerSystem");
                    var searcher = new ManagementObjectSearcher(query);                        
                    ManagementObjectCollection.ManagementObjectEnumerator itr = searcher.Get().GetEnumerator();
                    if (itr.MoveNext())
                    {
                        queryStr += itr.Current["Workgroup"] as string;
                    }
                }

                var root = new DirectoryEntry(queryStr);

                DirectoryEntries kids = root.Children;

                return (from DirectoryEntry node in kids where node.SchemaClassName == "Computer" select node.Name).ToList();
            }
            
            return new List<string> { Environment.MachineName };
        }
    }
}