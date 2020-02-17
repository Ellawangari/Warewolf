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
namespace Dev2.Activities.Specs.Composition
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.3.2.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class WorkflowExecutionLoggingFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
#line 1 "WorkflowExecutionLogging.feature"
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
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "WorkflowExecutionLogging", "      In order to get a detailed workflow execution log\r\n      As a warewolf user" +
                    "\r\n      I want to be able to list all entry to exit execution log points", ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        && (testRunner.FeatureContext.FeatureInfo.Title != "WorkflowExecutionLogging")))
            {
                global::Dev2.Activities.Specs.Composition.WorkflowExecutionLoggingFeature.FeatureSetup(null);
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Workflow execution Entry point detailed log")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "WorkflowExecutionLogging")]
        public virtual void WorkflowExecutionEntryPointDetailedLog()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Workflow execution Entry point detailed log", ((string[])(null)));
#line 6
  this.ScenarioSetup(scenarioInfo);
#line 7
          testRunner.Given("a valid workflow", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 8
          testRunner.And("workflow execution entry point detailed logs are logged", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 9
          testRunner.And("a workflow stops on error has no logs", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table592 = new TechTalk.SpecFlow.Table(new string[] {
                        "one",
                        "two",
                        "three"});
            table592.AddRow(new string[] {
                        "value 1",
                        "value 2",
                        "value 3"});
#line 10
          testRunner.Then("a detailed execution completed log entry is created", ((string)(null)), table592, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Workflow execution entry point detailed logs")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "WorkflowExecutionLogging")]
        public virtual void WorkflowExecutionEntryPointDetailedLogs()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Workflow execution entry point detailed logs", ((string[])(null)));
#line 14
this.ScenarioSetup(scenarioInfo);
#line 15
          testRunner.Given("a valid workflow", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 16
          testRunner.And("workflow execution entry point detailed logs are logged", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
          testRunner.When("a workflow stops on error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table593 = new TechTalk.SpecFlow.Table(new string[] {
                        "one",
                        "two",
                        "three"});
            table593.AddRow(new string[] {
                        "value 1",
                        "value 2",
                        "value 3"});
#line 18
          testRunner.Then("a detailed on error log entry is created", ((string)(null)), table593, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Workflow execution completed detailed logs")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "WorkflowExecutionLogging")]
        public virtual void WorkflowExecutionCompletedDetailedLogs()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Workflow execution completed detailed logs", ((string[])(null)));
#line 22
this.ScenarioSetup(scenarioInfo);
#line 23
          testRunner.Given("a valid workflow", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 24
          testRunner.And("workflow execution entry point detailed logs are logged", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 25
          testRunner.And("a workflow stops on error has no logs", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table594 = new TechTalk.SpecFlow.Table(new string[] {
                        "one",
                        "two",
                        "three"});
            table594.AddRow(new string[] {
                        "value 1",
                        "value 2",
                        "value 3"});
#line 26
          testRunner.Then("a detailed execution completed log entry is created", ((string)(null)), table594, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Workflow execution failure detailed logs")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "WorkflowExecutionLogging")]
        public virtual void WorkflowExecutionFailureDetailedLogs()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Workflow execution failure detailed logs", ((string[])(null)));
#line 30
this.ScenarioSetup(scenarioInfo);
#line 31
          testRunner.Given("a valid workflow", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 32
          testRunner.And("workflow execution entry point detailed logs are logged", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 33
          testRunner.And("a workflow stops on error has no logs", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
          testRunner.When("a workflow execution has an exception", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table595 = new TechTalk.SpecFlow.Table(new string[] {
                        "one",
                        "two",
                        "three"});
            table595.AddRow(new string[] {
                        "value 1",
                        "value 2",
                        "value 3"});
#line 35
          testRunner.Then("a detailed execution exception log entry is created", ((string)(null)), table595, "Then ");
#line 38
          testRunner.And("a detailed execution completed log entry is has no logs", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
