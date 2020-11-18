﻿/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2020 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System.Collections.Generic;
using System.Text;
using Dev2.Common;
using Dev2.Data.Interfaces.Enums;
using Warewolf.Driver.Persistence.Drivers;

namespace Warewolf.Driver.Persistence
{
    public class PersistenceExecution : IPersistenceExecution
    {
        public string ResumeJob(string jobId, bool overrideVariables, Dictionary<string, StringBuilder>  variables)
        {
            var scheduler = GetScheduler();
            return scheduler.ResumeJob(jobId, overrideVariables, variables);
        }

        public string CreateAndScheduleJob(enSuspendOption suspendOption, string suspendOptionValue, Dictionary<string, StringBuilder> values)
        {
            var scheduler = GetScheduler();
            var jobId = scheduler.ScheduleJob(suspendOption, suspendOptionValue, values);
            return jobId;
        }

        static IPersistenceScheduler GetScheduler()
        {
            if (Config.Persistence.PersistenceScheduler == nameof(Hangfire))
            {
                return new HangfireScheduler();
            }

            return null;
        }
    }
}