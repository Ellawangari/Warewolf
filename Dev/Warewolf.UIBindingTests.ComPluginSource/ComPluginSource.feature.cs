// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.1.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Warewolf.UIBindingTests.ComPluginSource
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class ComPluginSourceFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ComPluginSource.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner(null, 0);
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ComPluginSource", "\tIn order to create plugins\r\n\tAs a Warewolf User\r\n\tI want to be able to select dl" +
                    "ls as a source to be used", ProgrammingLanguage.CSharp, new string[] {
                        "ComPluginSource"});
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
                        && (testRunner.FeatureContext.FeatureInfo.Title != "ComPluginSource")))
            {
                Warewolf.UIBindingTests.ComPluginSource.ComPluginSourceFeature.FeatureSetup(null);
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
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("New COMPlugin Source File")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ComPluginSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("ComPluginSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("SpecFlow.Assist.Dynamic.dll")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v15." +
            "1.dll")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("Warewolf Studio.exe")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("Newtonsoft.Json.dll")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("Microsoft.Practices.Prism.SharedInterfaces.dll")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("System.Windows.Interactivity.dll")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("TechTalk.SpecFlow.dll")]
        public virtual void NewCOMPluginSourceFile()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("New COMPlugin Source File", new string[] {
                        "MSTest:DeploymentItem:SpecFlow.Assist.Dynamic.dll",
                        "MSTest:DeploymentItem:InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v15." +
                            "1.dll",
                        "MSTest:DeploymentItem:Warewolf_Studio.exe",
                        "MSTest:DeploymentItem:Newtonsoft.Json.dll",
                        "MSTest:DeploymentItem:Microsoft.Practices.Prism.SharedInterfaces.dll",
                        "MSTest:DeploymentItem:System.Windows.Interactivity.dll",
                        "MSTest:DeploymentItem:TechTalk.SpecFlow.dll"});
#line 20
this.ScenarioSetup(scenarioInfo);
#line 21
 testRunner.Given("I open New COMPlugin Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 22
 testRunner.Then("\"New COM Plugin Source\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 23
 testRunner.And("title is \"New COM Plugin Source\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Clicks"});
            table1.AddRow(new string[] {
                        "Development"});
#line 24
 testRunner.When("I click", ((string)(null)), table1, "When ");
#line 27
 testRunner.Then("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 28
 testRunner.When("I change Assembly to \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 29
 testRunner.Then("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 30
 testRunner.When("I change Assembly to \"SomethingElse\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 31
 testRunner.Then("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 32
 testRunner.When("I save as \"Testing Resource Save\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 33
 testRunner.Then("the save dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 34
 testRunner.Then("title is \"Testing Resource Save\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 35
 testRunner.And("\"Testing Resource Save\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Change Plugin Source Assembly Input")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ComPluginSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("ComPluginSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("SpecFlow.Assist.Dynamic.dll")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v15." +
            "1.dll")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("Warewolf Studio.exe")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("Newtonsoft.Json.dll")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("Microsoft.Practices.Prism.SharedInterfaces.dll")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("System.Windows.Interactivity.dll")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("TechTalk.SpecFlow.dll")]
        public virtual void ChangePluginSourceAssemblyInput()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Change Plugin Source Assembly Input", new string[] {
                        "MSTest:DeploymentItem:SpecFlow.Assist.Dynamic.dll",
                        "MSTest:DeploymentItem:InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v15." +
                            "1.dll",
                        "MSTest:DeploymentItem:Warewolf_Studio.exe",
                        "MSTest:DeploymentItem:Newtonsoft.Json.dll",
                        "MSTest:DeploymentItem:Microsoft.Practices.Prism.SharedInterfaces.dll",
                        "MSTest:DeploymentItem:System.Windows.Interactivity.dll",
                        "MSTest:DeploymentItem:TechTalk.SpecFlow.dll"});
#line 44
this.ScenarioSetup(scenarioInfo);
#line 45
 testRunner.Given("I open \"Test File\" plugin source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 46
 testRunner.Then("title is \"Test File\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 47
 testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
 testRunner.When("I click \"Development\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 49
 testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
 testRunner.When("I save Plugin source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("load all dependancies after filter cleared")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ComPluginSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("ComPluginSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("SpecFlow.Assist.Dynamic.dll")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v15." +
            "1.dll")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("Warewolf Studio.exe")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("Newtonsoft.Json.dll")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("Microsoft.Practices.Prism.SharedInterfaces.dll")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("System.Windows.Interactivity.dll")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItem("TechTalk.SpecFlow.dll")]
        public virtual void LoadAllDependanciesAfterFilterCleared()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("load all dependancies after filter cleared", new string[] {
                        "MSTest:DeploymentItem:SpecFlow.Assist.Dynamic.dll",
                        "MSTest:DeploymentItem:InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v15." +
                            "1.dll",
                        "MSTest:DeploymentItem:Warewolf_Studio.exe",
                        "MSTest:DeploymentItem:Newtonsoft.Json.dll",
                        "MSTest:DeploymentItem:Microsoft.Practices.Prism.SharedInterfaces.dll",
                        "MSTest:DeploymentItem:System.Windows.Interactivity.dll",
                        "MSTest:DeploymentItem:TechTalk.SpecFlow.dll"});
#line 59
this.ScenarioSetup(scenarioInfo);
#line 60
 testRunner.Given("I open New COMPlugin Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 61
 testRunner.And("DLLs is \"loading\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 62
 testRunner.And("I filter for \"Development\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 63
 testRunner.And("\"Development\" is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
 testRunner.When("I \"clear\" the filter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
