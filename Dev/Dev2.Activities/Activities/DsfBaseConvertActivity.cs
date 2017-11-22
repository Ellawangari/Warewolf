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
using System.Activities;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dev2;
using Dev2.Activities;
using Dev2.Activities.Debug;
using Dev2.Common;
using Dev2.Common.Interfaces.Core.Convertors.Base;
using Dev2.Common.Interfaces.Diagnostics.Debug;
using Dev2.Common.Interfaces.Enums.Enums;
using Dev2.Common.Interfaces.Toolbox;
using Dev2.Converters;
using Dev2.Data.TO;
using Dev2.Diagnostics;
using Dev2.Interfaces;
using Dev2.Validation;
using Warewolf.Core;
using Warewolf.Resource.Errors;
using Warewolf.Storage.Interfaces;


namespace Unlimited.Applications.BusinessDesignStudio.Activities

{
    [ToolDescriptorInfo("Data-BaseConversion", "Base Convert", ToolType.Native, "8999E59A-38A3-43BB-A98F-6090C5C9EA1E", "Dev2.Activities", "1.0.0.0", "", "Data", "/Warewolf.Studio.Themes.Luna;component/Images.xaml", "Tool_Data_Base_Convert")]
    public class DsfBaseConvertActivity : DsfActivityAbstract<string>, ICollectionActivity
    {

        #region Fields
        readonly Dev2BaseConversionFactory _fac = new Dev2BaseConversionFactory();

        #endregion

        #region Properties

        /// <summary>
        /// The property that holds all the convertions
        /// </summary>
        public IList<BaseConvertTO> ConvertCollection { get; set; }

        #endregion Properties

        #region Ctor

        /// <summary>
        /// The consructor for the activity 
        /// </summary>
        public DsfBaseConvertActivity()
            : base("Base Conversion")
        {
            ConvertCollection = new List<BaseConvertTO>();
        }

        #endregion Ctor

        public override List<string> GetOutputs()
        {
            return ConvertCollection.Select(to => to.ToExpression).ToList();
        }

        #region Overridden NativeActivity Methods

        
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
        }
        

        /// <summary>
        /// The execute method that is called when the activity is executed at run time and will hold all the logic of the activity
        /// </summary>       
        
        
        protected override void OnExecute(NativeActivityContext context)
            
        {
            var dataObject = context.GetExtension<IDSFDataObject>();

            ExecuteTool(dataObject, 0);
        }

        protected override void ExecuteTool(IDSFDataObject dataObject, int update)
        {


            var allErrors = new ErrorResultTO();

            InitializeDebug(dataObject);
            var env = dataObject.Environment;
            try
            {
                CleanArgs();

                int inputIndex = 1;
                int outputIndex = 1;

                foreach(var item in ConvertCollection.Where(a => !String.IsNullOrEmpty(a.FromExpression)))
                {
                    if(dataObject.IsDebugMode())
                    {
                        var debugItem = new DebugItem();
                        AddDebugItem(new DebugItemStaticDataParams("", inputIndex.ToString(CultureInfo.InvariantCulture)), debugItem);
                        AddDebugItem(new DebugEvalResult(item.FromExpression, "Convert", env, update), debugItem);
                        AddDebugItem(new DebugItemStaticDataParams(item.FromType, "From"), debugItem);
                        AddDebugItem(new DebugItemStaticDataParams(item.ToType, "To"), debugItem);
                        _debugInputs.Add(debugItem);
                        inputIndex++;
                    }

                    try
                    {
                        env.ApplyUpdate(item.FromExpression, TryConvertFunc(item, env, update), update);
                        IsSingleValueRule.ApplyIsSingleValueRule(item.FromExpression, allErrors);
                        if(dataObject.IsDebugMode())
                        {
                            var debugItem = new DebugItem();
                            AddDebugItem(new DebugItemStaticDataParams("", outputIndex.ToString(CultureInfo.InvariantCulture)), debugItem);
                            AddDebugItem(new DebugEvalResult(item.FromExpression, "", env, update), debugItem);
                            _debugOutputs.Add(debugItem);
                            outputIndex++;
                        }
                    }
                    catch(Exception e)
                    {
                        Dev2Logger.Error("DSFBaseConvert", e, GlobalConstants.WarewolfError);
                        allErrors.AddError(e.Message);
                        if(dataObject.IsDebugMode())
                        {
                            outputIndex++;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Dev2Logger.Error("DSFBaseConvert", e, GlobalConstants.WarewolfError);
                allErrors.AddError(e.Message);
            }
            finally
            {
                // Handle Errors
                var hasErrors = allErrors.HasErrors();
                if(hasErrors)
                {
                    DisplayAndWriteError("DsfBaseConvertActivity", allErrors);
                    var errorString = allErrors.MakeDisplayReady();
                    dataObject.Environment.AddError(errorString);
                }
                if(dataObject.IsDebugMode())
                {
                    DispatchDebugState(dataObject, StateType.Before, update);
                    DispatchDebugState(dataObject, StateType.After, update);
                }
            }
        }

        public override enFindMissingType GetFindMissingType()
        {
            return enFindMissingType.DataGridActivity;
        }

        #endregion

        Func<DataStorage.WarewolfAtom, DataStorage.WarewolfAtom> TryConvertFunc(BaseConvertTO item, IExecutionEnvironment env, int update)
        {
                return a =>
                {
                    var from = _fac.CreateConverter((enDev2BaseConvertType)Dev2EnumConverter.GetEnumFromStringDiscription(item.FromType, typeof(enDev2BaseConvertType)));
                    var to = _fac.CreateConverter((enDev2BaseConvertType)Dev2EnumConverter.GetEnumFromStringDiscription(item.ToType, typeof(enDev2BaseConvertType)));
                    var broker = _fac.CreateBroker(@from, to);
                    var value = a.ToString();
                    if(a.IsNothing)
                    {
                        throw new Exception(string.Format(ErrorResource.NullScalarValue, item.FromExpression));
                    }
                    if (String.IsNullOrEmpty(value))
                    {
                           
                        return DataStorage.WarewolfAtom.NewDataString("");
                            
                    }
                    var upper = broker.Convert(value);
                    var evalled = env.Eval(upper, update);
                    if (evalled.IsWarewolfAtomResult)
                    {
                        if (evalled is CommonFunctions.WarewolfEvalResult.WarewolfAtomResult warewolfAtomResult)
                        {
                            return warewolfAtomResult.Item;
                        }
                        return DataStorage.WarewolfAtom.Nothing;
                    }
                    return DataStorage.WarewolfAtom.NewDataString(CommonFunctions.evalResultToString(evalled));
                };

        }


        #region Private Methods

        void CleanArgs()
        {
            int count = 0;
            while (count < ConvertCollection.Count)
            {
                if (string.IsNullOrWhiteSpace(ConvertCollection[count].FromExpression))
                {
                    ConvertCollection.RemoveAt(count);
                }
                else
                {
                    count++;
                }
            }
        }

        #endregion Private Methods

        #region Get Debug Inputs/Outputs

        public override List<DebugItem> GetDebugInputs(IExecutionEnvironment dataList, int update)
        {
            foreach(IDebugItem debugInput in _debugInputs)
            {
                debugInput.FlushStringBuilder();
            }
            return _debugInputs;
        }

        public override List<DebugItem> GetDebugOutputs(IExecutionEnvironment dataList, int update)
        {
            foreach(IDebugItem debugOutput in _debugOutputs)
            {
                debugOutput.FlushStringBuilder();
            }
            return _debugOutputs;
        }

        #endregion

        #region Private Methods

        void InsertToCollection(IEnumerable<string> listToAdd, ModelItem modelItem)
        {
            var modelProperty = modelItem.Properties["ConvertCollection"];
            if (modelProperty != null)
            {
                var mic = modelProperty.Collection;

                if (mic != null)
                {
                    var listOfValidRows = ConvertCollection.Where(c => !c.CanRemove()).ToList();
                    if (listOfValidRows.Count > 0)
                    {
                        var baseConvertTo = ConvertCollection.Last(c => !c.CanRemove());
                        int startIndex = ConvertCollection.IndexOf(baseConvertTo) + 1;
                        foreach (string s in listToAdd)
                        {
                            mic.Insert(startIndex, new BaseConvertTO(s, ConvertCollection[startIndex - 1].FromType, ConvertCollection[startIndex - 1].ToType, string.Empty, startIndex + 1));
                            startIndex++;
                        }
                        CleanUpCollection(mic, modelItem, startIndex);
                    }
                    else
                    {
                        AddToCollection(listToAdd, modelItem);
                    }
                }
            }
        }

        void AddToCollection(IEnumerable<string> listToAdd, ModelItem modelItem)
        {
            var modelProperty = modelItem.Properties["ConvertCollection"];
            if (modelProperty != null)
            {
                var mic = modelProperty.Collection;

                if (mic != null)
                {
                    int startIndex = 0;
                    var firstRowConvertFromType = ConvertCollection[0].FromType;
                    var firstRowConvertToType = ConvertCollection[0].ToType;
                    mic.Clear();
                    foreach (string s in listToAdd)
                    {
                        mic.Add(new BaseConvertTO(s, firstRowConvertFromType, firstRowConvertToType, string.Empty, startIndex + 1));
                        startIndex++;
                    }
                    CleanUpCollection(mic, modelItem, startIndex);
                }
            }
        }

        void CleanUpCollection(ModelItemCollection mic, ModelItem modelItem, int startIndex)
        {
            if (startIndex < mic.Count)
            {
                mic.RemoveAt(startIndex);
            }
            mic.Add(new BaseConvertTO(string.Empty, "Text", "Base 64", string.Empty, startIndex + 1));
            var modelProperty = modelItem.Properties["DisplayName"];
            if (modelProperty != null)
            {
                modelProperty.SetValue(CreateDisplayName(modelItem, startIndex + 1));
            }
        }

        string CreateDisplayName(ModelItem modelItem, int count)
        {
            var modelProperty = modelItem.Properties["DisplayName"];
            if (modelProperty != null)
            {
                var currentName = modelProperty.ComputedValue as string;
                if (currentName != null && currentName.Contains("(") && currentName.Contains(")"))
                {
                    currentName = currentName.Remove(currentName.Contains(" (") ? currentName.IndexOf(" (", StringComparison.Ordinal) : currentName.IndexOf("(", StringComparison.Ordinal));
                }
                currentName = currentName + " (" + (count - 1) + ")";
                return currentName;
            }

            return string.Empty;
        }

        #endregion

        #region Get ForEach Inputs/Outputs

        public override void UpdateForEachInputs(IList<Tuple<string, string>> updates)
        {

            foreach(Tuple<string, string> t in updates)
            {
                // locate all updates for this tuple
                var t1 = t;
                var items = ConvertCollection.Where(c => !string.IsNullOrEmpty(c.FromExpression) && c.FromExpression.Contains(t1.Item1));

                // issues updates
                foreach(var a in items)
                {
                    a.FromExpression = a.FromExpression.Replace(t.Item1, t.Item2);
                }
            }
        }

        public override void UpdateForEachOutputs(IList<Tuple<string, string>> updates)
        {

            foreach(Tuple<string, string> t in updates)
            {

                // locate all updates for this tuple
                //TODO : This need to be changed when the expanded version comes in because the user can set the ToExpression
                var t1 = t;
                var items = ConvertCollection.Where(c => !string.IsNullOrEmpty(c.FromExpression) && c.FromExpression.Contains(t1.Item1));

                // issues updates
                foreach(var a in items)
                {
                    a.ToExpression = a.FromExpression.Replace(t.Item1, t.Item2);
                }
            }

        }

        #endregion

        #region GetForEachInputs/Outputs

        public override IList<DsfForEachItem> GetForEachInputs()
        {
            var result = new List<DsfForEachItem>();

            
            foreach(var item in ConvertCollection)
            
            {
                if(!string.IsNullOrEmpty(item.FromExpression) && item.FromExpression.Contains("[["))
                {
                    result.Add(new DsfForEachItem { Name = item.FromExpression, Value = item.FromExpression });
                }
            }

            return result;
        }

        public override IList<DsfForEachItem> GetForEachOutputs()
        {
            var result = new List<DsfForEachItem>();

            
            foreach(var item in ConvertCollection)
            
            {
                if(!string.IsNullOrEmpty(item.FromExpression) && item.FromExpression.Contains("[["))
                {
                    result.Add(new DsfForEachItem { Name = item.FromExpression, Value = item.FromExpression });
                }
            }

            return result;
        }

        #endregion

        #region Implementation of ICollectionActivity

        public int GetCollectionCount() => throw new NotImplementedException();
        public void AddListToCollection(IList<string> listToAdd, bool overwrite, ModelItem modelItem) => throw new NotImplementedException();

        #endregion
    }
}
