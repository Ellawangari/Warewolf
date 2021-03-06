﻿using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Warewolf.UI.Tests.WorkflowTab.Tools.Data.DataToolsUIMapClasses;
using Warewolf.UI.Tests.WorkflowTab.WorkflowTabUIMapClasses;
using Warewolf.UI.Tests.Explorer.ExplorerUIMapClasses;
using Warewolf.UI.Tests.DialogsUIMapClasses;

namespace Warewolf.UI.Tests.WindowsDesignSurfaceContextMenu
{
    [CodedUITest]
    public class DesignSurfaceContextMenuTests
    {
        [TestMethod]
        public void CopyAndPasteWorkflowToItselfDoesNotCopy()
        {
            UIMap.Click_NewWorkflow_RibbonButton();
            ExplorerUIMap.Filter_Explorer("stackoverflowTestWorkflow");
            WorkflowTabUIMap.Drag_Explorer_Localhost_First_Item_Onto_Workflow_Design_Surface();
            Assert.IsTrue(WorkflowTabUIMap.MainStudioWindow.TabManSplitPane.TabMan.WorkflowTab.WorkflowSurfaceContext.ContentDockManagerCustom.WorkflowDesignerView.DesignerView.ScrollViewerPane.ActivityTypeDesigner.WorkflowItemPresenter.Flowchart.stackOverflowTestWF.Exists);
            WorkflowTabUIMap.RightClick_StackOverFlowService_OnDesignSurface();
            UIMap.Select_Copy_FromContextMenu();
            ExplorerUIMap.Open_ExplorerFirstItem_From_ExplorerContextMenu();
            DataToolsUIMap.RightClick_AssignOnDesignSurface();
            UIMap.Select_Paste_FromContextMenu();
            var controlExistsNow = UIMap.ControlExistsNow(WorkflowTabUIMap.MainStudioWindow.TabManSplitPane.TabMan.WorkflowTab.WorkflowSurfaceContext.ContentDockManagerCustom.WorkflowDesignerView.DesignerView.ScrollViewerPane.ActivityTypeDesigner.WorkflowItemPresenter.Flowchart.stackOverflowTestWF);
            Assert.IsFalse(controlExistsNow);
            UIMap.Click_Close_Workflow_Tab_Button();
            DialogsUIMap.Click_MessageBox_Yes();
            Assert.IsTrue(WorkflowTabUIMap.MainStudioWindow.TabManSplitPane.TabMan.WorkflowTab.WorkflowSurfaceContext.ContentDockManagerCustom.WorkflowDesignerView.DesignerView.ScrollViewerPane.ActivityTypeDesigner.WorkflowItemPresenter.Flowchart.stackOverflowTestWF.Exists);
        }

        #region Additional test attributes

        [TestInitialize()]
        public void MyTestInitialize()
        {
            UIMap.SetPlaybackSettings();
            UIMap.AssertStudioIsRunning();
        }

        public UIMap UIMap
        {
            get
            {
                if (_UIMap == null)
                {
                    _UIMap = new UIMap();
                }

                return _UIMap;
            }
        }

        private UIMap _UIMap;

        WorkflowTabUIMap WorkflowTabUIMap
        {
            get
            {
                if (_WorkflowTabUIMap == null)
                {
                    _WorkflowTabUIMap = new WorkflowTabUIMap();
                }

                return _WorkflowTabUIMap;
            }
        }

        private WorkflowTabUIMap _WorkflowTabUIMap;

        ExplorerUIMap ExplorerUIMap
        {
            get
            {
                if (_ExplorerUIMap == null)
                {
                    _ExplorerUIMap = new ExplorerUIMap();
                }

                return _ExplorerUIMap;
            }
        }

        private ExplorerUIMap _ExplorerUIMap;

        DataToolsUIMap DataToolsUIMap
        {
            get
            {
                if (_DataToolsUIMap == null)
                {
                    _DataToolsUIMap = new DataToolsUIMap();
                }

                return _DataToolsUIMap;
            }
        }

        private DataToolsUIMap _DataToolsUIMap;

        DialogsUIMap DialogsUIMap
        {
            get
            {
                if (_DialogsUIMap == null)
                {
                    _DialogsUIMap = new DialogsUIMap();
                }

                return _DialogsUIMap;
            }
        }

        private DialogsUIMap _DialogsUIMap;

        #endregion
    }
}
