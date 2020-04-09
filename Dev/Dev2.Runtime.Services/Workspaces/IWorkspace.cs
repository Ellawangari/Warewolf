/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2020 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Warewolf.Esb;

namespace Dev2.Workspaces
{
    public interface IWorkspace : IWorkspaceBase, ISerializable
    {
        Guid ID
        {
            get;
        }

        IList<IWorkspaceItem> Items
        {
            get;
        }
    }
}
