﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.3.2.0
//      SpecFlow Generator Version:2.3.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Dev2.Activities.Specs.Scheduler
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.3.2.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class SchedulerFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
#line 1 "Scheduler.feature"
#line hidden
        
        public virtual Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext
        {
            get
            {
                return this._testContext;
            }
            set
            {
                this._testContext = value;
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner(null, 0);
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Scheduler", "\tIn order to schedule workflows\r\n\tAs a Warewolf user\r\n\tI want to setup schedules", ProgrammingLanguage.CSharp, new string[] {
                        "Scheduler"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Scheduler")))
            {
                global::Dev2.Activities.Specs.Scheduler.SchedulerFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Microsoft.VisualStudio.TestTools.UnitTesting.TestContext>(TestContext);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Schedule with history")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Scheduler")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Scheduler")]
        public virtual void ScheduleWithHistory()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Schedule with history", ((string[])(null)));
#line 8
 this.ScenarioSetup(scenarioInfo);
#line 9
  testRunner.Given("I have a schedule \"ScheduleWithHistory\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 10
  testRunner.And("\"ScheduleWithHistory\" executes an Workflow \"Hello World\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
  testRunner.And("task history \"Number of history records to load\" is \"2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
  testRunner.And("the task status \"Status\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
  testRunner.And("\"Diceroll00\" has a username of \".\\LocalSchedulerAdmin\" and a Password of \"987Sche" +
                    "d#@!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table919 = new TechTalk.SpecFlow.Table(new string[] {
                        "ScheduleType",
                        "Interval",
                        "StartDate",
                        "StartTime",
                        "Recurs",
                        "RecursInterval",
                        "Delay",
                        "DelayInterval",
                        "Repeat",
                        "RepeatInterval",
                        "ExpireDate",
                        "ExpireTime",
                        "ResourceId"});
            table919.AddRow(new string[] {
                        "On a schedule",
                        "Daily",
                        "2020/01/01",
                        "15:40:44",
                        "1",
                        "day",
                        "1",
                        "hour",
                        "1",
                        "hour",
                        "2020/01/02",
                        "15:40:15",
                        ""});
#line 14
  testRunner.And("\"ScheduleWithHistory\" has a Schedule of", ((string)(null)), table919, "And ");
#line 17
  testRunner.When("the \"ScheduleWithHistory\" is executed \"1\" times", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 18
  testRunner.Then("the Schedule task has \"NO\" error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 19
  testRunner.Then("the schedule status is \"Success\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 20
  testRunner.And("\"ScheduleWithHistory\" has \"2\" row of history", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Creating task with schedule status is disabled")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Scheduler")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Scheduler")]
        public virtual void CreatingTaskWithScheduleStatusIsDisabled()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Creating task with schedule status is disabled", ((string[])(null)));
#line 22
 this.ScenarioSetup(scenarioInfo);
#line 23
  testRunner.Given("I have a schedule \"Diceroll00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 24
  testRunner.And("\"Diceroll00\" executes an Workflow \"Hello World\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 25
  testRunner.And("task history \"Number of history records to load\" is \"2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 26
  testRunner.And("the task status \"Status\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
  testRunner.And("\"Diceroll00\" has a username of \".\\LocalSchedulerAdmin\" and a Password of \"987Sche" +
                    "d#@!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table920 = new TechTalk.SpecFlow.Table(new string[] {
                        "ScheduleType",
                        "Interval",
                        "StartDate",
                        "StartTime",
                        "Recurs",
                        "RecursInterval",
                        "Delay",
                        "DelayInterval",
                        "Repeat",
                        "RepeatInterval",
                        "ExpireDate",
                        "ExpireTime",
                        "ResourceId"});
            table920.AddRow(new string[] {
                        "On a schedule",
                        "\"Daily\"",
                        "2020/01/01",
                        "15:40:44",
                        "1",
                        "day",
                        "1",
                        "hour",
                        "1",
                        "hour",
                        "2020/01/02",
                        "15:40:15",
                        ""});
#line 28
  testRunner.And("\"Diceroll00\" has a Schedule of", ((string)(null)), table920, "And ");
#line 31
  testRunner.When("the \"Diceroll00\" is executed \"1\" times", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 32
  testRunner.Then("the Schedule task has \"AN\" error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Setting schedule task \"At log on\"")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Scheduler")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Scheduler")]
        public virtual void SettingScheduleTaskAtLogOn()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Setting schedule task \"At log on\"", ((string[])(null)));
#line 34
 this.ScenarioSetup(scenarioInfo);
#line 35
  testRunner.Given("I have a schedule \"Diceroll1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 36
  testRunner.And("\"Diceroll1\" executes an Workflow \"Hello World\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
  testRunner.And("task history \"Number of history records to load\" is \"2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 38
  testRunner.And("the task status \"Status\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 39
  testRunner.And("\"Diceroll1\" has a username of \".\\LocalSchedulerAdmin\" and a Password of \"987Sched" +
                    "#@!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table921 = new TechTalk.SpecFlow.Table(new string[] {
                        "ScheduleType",
                        "Delay",
                        "DelayInterval",
                        "Repeat",
                        "RepeatInterval",
                        "ExpireDate",
                        "ExpireTime",
                        "ResourceId"});
            table921.AddRow(new string[] {
                        "At log on",
                        "1",
                        "hour",
                        "1",
                        "hour",
                        "2020/01/02",
                        "15:40:15",
                        ""});
#line 40
  testRunner.And("\"Diceroll1\" has a Schedule of", ((string)(null)), table921, "And ");
#line 43
  testRunner.Then("the Schedule task has \"NO\" error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 44
  testRunner.When("the \"Diceroll1\" is executed \"1\" times", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 45
  testRunner.Then("the Schedule task has \"NO\" error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 46
  testRunner.Then("the schedule status is \"Success\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 47
  testRunner.And("\"Diceroll1\" has \"2\" row of history", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Schedule the task with Incorrect username or password")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Scheduler")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Scheduler")]
        public virtual void ScheduleTheTaskWithIncorrectUsernameOrPassword()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Schedule the task with Incorrect username or password", ((string[])(null)));
#line 49
 this.ScenarioSetup(scenarioInfo);
#line 50
  testRunner.Given("I have a schedule \"Diceroll1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 51
  testRunner.And("\"Diceroll1\" executes an Workflow \"Hello World\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 52
  testRunner.And("task history \"Number of history records to load\" is \"2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
  testRunner.And("the task status \"Status\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 54
  testRunner.And("\"Diceroll1\" has a username of \"bobthebuilder\" and a Password of \"987Sched#@!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table922 = new TechTalk.SpecFlow.Table(new string[] {
                        "ScheduleType",
                        "Delay",
                        "DelayInterval",
                        "Repeat",
                        "RepeatInterval",
                        "ExpireDate",
                        "ExpireTime",
                        "ResourceId"});
            table922.AddRow(new string[] {
                        "At log on",
                        "1",
                        "hour",
                        "1",
                        "hour",
                        "2020/01/02",
                        "15:40:15",
                        ""});
#line 55
  testRunner.And("\"Diceroll1\" has a Schedule of", ((string)(null)), table922, "And ");
#line 58
  testRunner.Then("the Schedule task has \"AN\" error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Schedule with LocalUser")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Scheduler")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Scheduler")]
        public virtual void ScheduleWithLocalUser()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Schedule with LocalUser", ((string[])(null)));
#line 60
 this.ScenarioSetup(scenarioInfo);
#line 61
  testRunner.Given("I have a schedule \"LocalUserSchedule\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 62
  testRunner.And("\"LocalUserSchedule\" executes an Workflow \"Hello World\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 63
  testRunner.And("task history \"Number of history records to load\" is \"2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
  testRunner.And("the task status \"Status\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 65
  testRunner.And("\"LocalUserSchedule\" has a username of \".\\LocalSchedulerAdmin\" and a Password of \"" +
                    "987Sched#@!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table923 = new TechTalk.SpecFlow.Table(new string[] {
                        "ScheduleType",
                        "Interval",
                        "StartDate",
                        "StartTime",
                        "Recurs",
                        "RecursInterval",
                        "Delay",
                        "DelayInterval",
                        "Repeat",
                        "RepeatInterval",
                        "ExpireDate",
                        "ExpireTime",
                        "ResourceId"});
            table923.AddRow(new string[] {
                        "On a schedule",
                        "Daily",
                        "2020/01/01",
                        "15:40:44",
                        "1",
                        "day",
                        "1",
                        "hour",
                        "1",
                        "hour",
                        "2020/01/02",
                        "15:40:15",
                        ""});
#line 66
  testRunner.And("\"LocalUserSchedule\" has a Schedule of", ((string)(null)), table923, "And ");
#line 69
  testRunner.When("the \"LocalUserSchedule\" is executed \"1\" times", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 70
  testRunner.Then("the Schedule task has \"NO\" error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 71
  testRunner.Then("the schedule status is \"Success\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 72
  testRunner.And("\"LocalUserSchedule\" has \"2\" row of history", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Schedule with ErrorInDebug")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Scheduler")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Scheduler")]
        public virtual void ScheduleWithErrorInDebug()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Schedule with ErrorInDebug", ((string[])(null)));
#line 74
 this.ScenarioSetup(scenarioInfo);
#line 75
  testRunner.Given("I have a schedule \"ScheduleWithError\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 76
  testRunner.And("\"ScheduleWithError\" executes an Workflow \"moocowimpi\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
  testRunner.And("task history \"Number of history records to load\" is \"2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 78
  testRunner.And("the task status \"Status\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 79
  testRunner.And("\"ScheduleWithError\" has a username of \".\\LocalSchedulerAdmin\" and a Password of \"" +
                    "987Sched#@!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table924 = new TechTalk.SpecFlow.Table(new string[] {
                        "ScheduleType",
                        "Interval",
                        "StartDate",
                        "StartTime",
                        "Recurs",
                        "RecursInterval",
                        "Delay",
                        "DelayInterval",
                        "Repeat",
                        "RepeatInterval",
                        "ExpireDate",
                        "ExpireTime",
                        "ResourceId"});
            table924.AddRow(new string[] {
                        "On a schedule",
                        "Daily",
                        "2020/01/01",
                        "15:40:44",
                        "1",
                        "day",
                        "1",
                        "hour",
                        "1",
                        "hour",
                        "2020/01/02",
                        "15:40:15",
                        "acb75027-ddeb-47d7-814e-a54c37247ec2"});
#line 80
  testRunner.And("\"ScheduleWithError\" has a Schedule of", ((string)(null)), table924, "And ");
#line 83
  testRunner.When("the \"ScheduleWithError\" is executed \"1\" times", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 84
  testRunner.Then("the Schedule task has \"AN\" error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 85
  testRunner.Then("the schedule status is \"Failure\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Schedule Workflow with success")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Scheduler")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Scheduler")]
        public virtual void ScheduleWorkflowWithSuccess()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Schedule Workflow with success", ((string[])(null)));
#line 87
 this.ScenarioSetup(scenarioInfo);
#line 88
  testRunner.Given("I have a schedule \"ScheduleAssignOutput\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 89
  testRunner.And("\"ScheduleAssignOutput\" executes an Workflow \"AssignOutput\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 90
  testRunner.And("task history \"Number of history records to load\" is \"2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 91
  testRunner.And("the task status \"Status\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 92
  testRunner.And("\"ScheduleAssignOutput\" has a username of \".\\LocalSchedulerAdmin\" and a Password o" +
                    "f \"987Sched#@!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table925 = new TechTalk.SpecFlow.Table(new string[] {
                        "ScheduleType",
                        "Interval",
                        "StartDate",
                        "StartTime",
                        "Recurs",
                        "RecursInterval",
                        "Delay",
                        "DelayInterval",
                        "Repeat",
                        "RepeatInterval",
                        "ExpireDate",
                        "ExpireTime",
                        "ResourceId"});
            table925.AddRow(new string[] {
                        "On a schedule",
                        "Daily",
                        "2020/01/01",
                        "15:40:44",
                        "1",
                        "day",
                        "1",
                        "hour",
                        "1",
                        "hour",
                        "2020/01/02",
                        "15:40:15",
                        "e7ea5196-33f7-4e0e-9d66-44bd67528a96"});
#line 93
  testRunner.And("\"ScheduleAssignOutput\" has a Schedule of", ((string)(null)), table925, "And ");
#line 96
  testRunner.When("the \"ScheduleAssignOutput\" is executed \"1\" times", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 97
  testRunner.Then("the Schedule task has \"NO\" error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 98
  testRunner.Then("the schedule status is \"Success\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
