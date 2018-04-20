/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2018 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Dev2.Common;
using Dev2.Common.Interfaces;
using Dev2.Data.Exceptions;
using Dev2.Data.Interfaces;
using Dev2.Data.Interfaces.Enums;
using Dev2.Data.TO;
using Dev2.Data.Util;
using Dev2.DataList.Contract;
using Warewolf.Resource.Errors;


namespace Dev2.Data.Parsers
{
    public class Dev2DataLanguageParser : IDev2DataLanguageParser, IDev2StudioDataLanguageParser
    {
        const string CdataStart = @"<![CDATA[";
        const string CdataEnd = @"]]>";
        static volatile ConcurrentDictionary<Tuple<string, string>, IList<IIntellisenseResult>> _payloadCache = new ConcurrentDictionary<Tuple<string, string>, IList<IIntellisenseResult>>();
        static volatile ConcurrentDictionary<string, IList<IIntellisenseResult>> _expressionCache = new ConcurrentDictionary<string, IList<IIntellisenseResult>>();

        static IParserHelper _parserHelper = new ParserHelperUtil();
        static ICommonRecordSetUtil _recordSetUtil = new CommonRecordSetUtil();

        #region Public Methods

        public IList<IIntellisenseResult> ParseExpressionIntoParts(string expression, IList<IDev2DataLanguageIntellisensePart> parts) => WrapAndClear(() =>
        {
            if (string.IsNullOrEmpty(expression) || parts == null)
            {
                return new List<IIntellisenseResult>();
            }
            var canCache = Regex.Matches(expression, "\\[\\[").Count == 1;
            if (canCache && _expressionCache.ContainsKey(expression))
            {
                return _expressionCache[expression];
            }


            var result = PartsGeneration(expression, parts, true);
            if (result != null && canCache && result.All(a => a.Type != enIntellisenseResultType.Error))
            {
                try
                {
                    _expressionCache.TryAdd(expression, result);
                }

                catch (Exception e)
                {
                    Dev2Logger.Warn(e.Message, "Warewolf Warn");
                }
            }

            return result;
        }, _expressionCache);

        public IList<IIntellisenseResult> ParseDataLanguageForIntellisense(string payload, string dataList) => ParseDataLanguageForIntellisense(payload, dataList, false, null, false);

        public IList<IIntellisenseResult> ParseDataLanguageForIntellisense(string payload, string dataList, bool addCompleteParts) => ParseDataLanguageForIntellisense(payload, dataList, addCompleteParts, null, false);

        public IList<IIntellisenseResult> ParseDataLanguageForIntellisense(string payload, string dataList, bool addCompleteParts, IIntellisenseFilterOpsTO filterTo) => ParseDataLanguageForIntellisense(payload, dataList, addCompleteParts, filterTo, false);

        public IList<IIntellisenseResult> ParseDataLanguageForIntellisense(string payload, string dataList, bool addCompleteParts, IIntellisenseFilterOpsTO filterTo, bool isFromIntellisense)
        {
            return WrapAndClear(() =>
            {
                var key = new Tuple<string, string>(payload, dataList);
                if (string.IsNullOrEmpty(payload))
                {
                    return new List<IIntellisenseResult>();
                }
                if (_payloadCache.ContainsKey(key))
                {
                    return _payloadCache[key];
                }
                var parts = DataListFactory.GenerateIntellisensePartsFromDataList(dataList, filterTo);

                IList<IDev2DataLanguageIntellisensePart> additionalParts = new List<IDev2DataLanguageIntellisensePart>();
                if (filterTo != null && filterTo.FilterType == enIntellisensePartType.RecordsetsOnly)
                {
                    additionalParts = DataListFactory.GenerateIntellisensePartsFromDataList(dataList, new IntellisenseFilterOpsTO { FilterCondition = filterTo.FilterCondition, FilterType = enIntellisensePartType.None });
                }
                var result = PartsGeneration(payload, parts, addCompleteParts, isFromIntellisense, additionalParts);
                if (result != null && result.Count > 0 && result.All(a => a.Type != enIntellisenseResultType.Error))
                {
                    try
                    {
                        _payloadCache.TryAdd(key, result);
                    }

                    catch (Exception e)
                    {
                        Dev2Logger.Warn(e.Message, "Warewolf Warn");
                    }
                }

                return result;
            }, _payloadCache);

        }

        T WrapAndClear<T, U>(Func<T> runFunc, ConcurrentDictionary<U, T> clearIfException)
        {
            try
            {
                return runFunc();
            }
            catch (Exception e)
            {
                Dev2Logger.Error(e, GlobalConstants.WarewolfError);
                clearIfException.Clear();
                throw;
            }
        }

        public IList<string> ParseForActivityDataItems(string payload)
        {
            if (string.IsNullOrEmpty(payload))
            {
                return new List<string>();
            }

            IList<string> result = new List<string>();

            var parserList = MakeParts(payload);

            foreach (var parseObject in parserList)
            {
                var parseRef = parseObject;
                while (parseRef != null)
                {
                    if (!result.Contains(parseRef.Payload))
                    {
                        result.Add(parseRef.Payload);
                    }
                    parseRef = parseRef.Child;
                }
            }

            return result;
        }

        #endregion

        #region Private Methods

        IList<IIntellisenseResult> PartsGeneration(string payload, IList<IDev2DataLanguageIntellisensePart> parts, bool addCompleteParts, bool isFromIntellisense = false, IList<IDev2DataLanguageIntellisensePart> additionalParts = null)
        {
            IList<IIntellisenseResult> result = new List<IIntellisenseResult>();
            try
            {
                if (payload.Contains(CdataStart))
                {
                    payload = payload.Replace(CdataStart, "");
                    var idx = payload.LastIndexOf(CdataEnd, StringComparison.Ordinal);
                    payload = payload.Substring(0, idx);
                }

                if (payload.Equals("[[]]"))
                {
                    result.Add(IntellisenseFactory.CreateErrorResult(0, 4, null, ErrorResource.VariableIsMissing, enIntellisenseErrorCode.SyntaxError, true));
                    return result;
                }

                if (payload.Contains(DataListUtil.OpeningSquareBrackets))
                {
                    var rootItems = MakeParts(payload, addCompleteParts);
                    IParseTO magicRegion = null;
                    IList<IParseTO> evalParts = new List<IParseTO>();

                    magicRegion = ProcessRootItems(rootItems, magicRegion, evalParts);
                    if (magicRegion != null)
                    {
                        result = isFromIntellisense ? ExtractActualIntellisenseOptions(magicRegion, parts, false) : ExtractIntellisenseOptions(magicRegion, parts, false);
                    }
                    evalParts
                        .ToList()
                        .ForEach(evalPart =>
                        {
                            var tmp = ExtractIntellisenseOptions(evalPart, parts, !isFromIntellisense && addCompleteParts, additionalParts);
                            if (tmp != null)
                            {
                                result = result.Union(tmp).ToList();
                            }
                        });
                }
            }
            catch (Dev2DataLanguageParseError e)
            {
                var p = IntellisenseFactory.CreateDataListValidationScalarPart(payload);
                result.Add(IntellisenseFactory.CreateErrorResult(e.StartIndex, e.EndIndex, p, e.Message, e.ErrorCode, true));
            }
            return result;
        }

        private static IParseTO ProcessRootItems(IList<IParseTO> rootItems, IParseTO magicRegion, IList<IParseTO> evalParts)
        {
            rootItems
                .ToList()
                .ForEach(rootItem =>
                {
                    var eval = rootItem;
                    while (eval != null)
                    {
                        if (eval.HangingOpen)
                        {
                            magicRegion = eval;
                        }
                        if (!eval.HangingOpen && eval != magicRegion)
                        {
                            evalParts.Add(eval);
                        }
                        eval = eval.Child;
                    }
                });
            return magicRegion;
        }

        public IList<IParseTO> MakeParts(string payload) => MakeParts(payload, false);

        public IList<IParseTO> MakeParts(string payload, bool addCompleteParts)
        {
            if (string.IsNullOrEmpty(payload))
            {
                return new List<IParseTO>();
            }

            var prev = '\0';
            var region = new StringBuilder();
            var openRegion = false;
            IParseTO currentNode = new ParseTO { Parent = null, HangingOpen = true };
            var root = currentNode;
            int i;
            payload = payload.Replace("]].[[", "]][[");
            IList<IParseTO> result = new List<IParseTO>();

            for (i = 0; i < payload.Length; i++)
            {
                var cur = payload[i];
                var shouldAddToRegion = ShouldAddToRegion(payload, cur, prev, i, true, '[');
                shouldAddToRegion = ShouldAddToRegion(payload, cur, prev, i, shouldAddToRegion, ']');

                if (cur == '[' && prev == '[')
                {
                    currentNode = openRegion ? CurrentNode(currentNode, region, i) : ParseTO(currentNode, i, result, ref root, ref openRegion);
                    cur = '\0';
                }

                if (cur == ']' && prev == ']')
                {
                    openRegion = ProcessOpenRegion(payload, openRegion, i, ref currentNode, ref region, ref cur);
                }

                if (openRegion && shouldAddToRegion && cur != '\0')
                {
                    region.Append(cur);
                }
                prev = cur;
            }

            if (openRegion)
            {
                currentNode.EndIndex = i - 1;
                currentNode.Payload = region.ToString();
            }

            if (!result.Contains(root))
            {
                result.Add(root);
            }

            if (root.HangingOpen && addCompleteParts)
            {
                throw new Dev2DataLanguageParseError(ErrorResource.InvalidCloseRegion, 0, payload.Length, enIntellisenseErrorCode.SyntaxError);
            }
            return result;
        }

        static IParseTO ParseTO(IParseTO currentNode, int i, IList<IParseTO> result, ref IParseTO root, ref bool openRegion)
        {
            if (currentNode == root && !root.HangingOpen)
            {
                IParseTO newRoot = new ParseTO { HangingOpen = true, Parent = null, StartIndex = i, EndIndex = -1 };
                result.Add(root);
                root = newRoot;
                currentNode = root;
            }
            openRegion = true;
            currentNode.StartIndex = i + 1;
            return currentNode;
        }

        static bool ProcessOpenRegion(string payload, bool openRegion, int i, ref IParseTO currentNode, ref StringBuilder region, ref char cur) => _parserHelper.ProcessOpenRegion(payload, openRegion, i, ref currentNode, ref region, ref cur);

        static IParseTO CurrentNode(IParseTO currentNode, StringBuilder region, int i) => _parserHelper.CurrentNode(currentNode, region, i);

        static bool ShouldAddToRegion(string payload, char cur, char prev, int i, bool shouldAddToRegion, char charToCheck) => _parserHelper.ShouldAddToRegion(payload, cur, prev, i, shouldAddToRegion, charToCheck);

        IList<IIntellisenseResult> ExtractActualIntellisenseOptions(IParseTO payload, IEnumerable<IDev2DataLanguageIntellisensePart> refParts, bool addCompleteParts)
        {
            var tmp = new StringBuilder(payload.Payload);
            IList<IIntellisenseResult> result = new List<IIntellisenseResult>();

            if (payload != null)

            {
                var parts = tmp.ToString().Split('.');
                var search = parts[0].ToLower();
                var isRs = search.Contains(DataListUtil.RecordsetIndexOpeningBracket);

                if (search.Contains(DataListUtil.RecordsetIndexOpeningBracket))
                {
                    isRs = true;
                    var pos = search.IndexOf(DataListUtil.RecordsetIndexOpeningBracket, StringComparison.Ordinal);
                    search = search.Substring(0, search.Length - (search.Length - pos));
                }

                TryExtractActualResults(payload, refParts, addCompleteParts, result, parts, search, isRs);
            }

            IList<IIntellisenseResult> realResults = new List<IIntellisenseResult>();

            result
                .ToList()
                .ForEach(r =>
                {

                    var addToFinal = true;

                    realResults
                        .ToList()
                        .ForEach(rr =>
                        {
                            if (rr.Option.DisplayValue == r.Option.DisplayValue)
                            {
                                addToFinal = false;
                            }

                        });

                    if (addToFinal)
                    {
                        realResults.Add(r);
                    }

                });

            return result;
        }

        private void TryExtractActualResults(IParseTO payload, IEnumerable<IDev2DataLanguageIntellisensePart> refParts, bool addCompleteParts, IList<IIntellisenseResult> result, string[] parts, string search, bool isRs)
        {
            try
            {
                var results = CreateResultsGeneric(refParts, payload, parts.Length == 1 ? search : parts[1], addCompleteParts);

                if (parts.Length == 2)
                {
                    CheckForFieldOptions(result, parts, results);
                }
                else
                {
                    foreach (IIntellisenseResult res in results)
                    {
                        result.Add(res);
                    }
                }
            }
            catch (Dev2DataLanguageParseError e)
            {
                result.Add(AddErrorToResults(isRs, parts[0], e, !payload.HangingOpen));
            }
        }

        private static void CheckForFieldOptions(IList<IIntellisenseResult> result, string[] parts, IEnumerable<IIntellisenseResult> results)
        {
            var cmp = parts[1].ToLower();

            foreach (IIntellisenseResult res in results)
            {
                if (res.Option.Field.ToLower().IndexOf(cmp, StringComparison.Ordinal) >= 0)
                {
                    result.Add(res);
                }
            }
        }

        IEnumerable<IIntellisenseResult> CreateResultsGeneric(IEnumerable<IDev2DataLanguageIntellisensePart> refParts, IParseTO payload, string search, bool addCompleteParts)
        {
            IList<IIntellisenseResult> result = new List<IIntellisenseResult>();

            if (payload.HangingOpen)
            {
                var hasIndex = false;
                var openBraceIndex = search.LastIndexOf(DataListUtil.RecordsetIndexOpeningBracket, StringComparison.Ordinal);
                var closeBraceIndex = search.LastIndexOf(DataListUtil.RecordsetIndexClosingBracket, StringComparison.Ordinal);
                if (openBraceIndex != -1 && openBraceIndex < search.Length && closeBraceIndex > openBraceIndex)
                {
                    hasIndex = true;
                }

                if (payload.Payload.Contains(DataListUtil.RecordsetIndexOpeningBracket) && !payload.Payload.Contains(DataListUtil.RecordsetIndexClosingBracket) && payload.Child == null && !hasIndex)
                {
                    AddIndex(refParts, payload, search, result);
                }
                else
                {
                    AddItems(refParts, payload, search, addCompleteParts, result);
                }
            }

            return result;
        }

        private static void AddItems(IEnumerable<IDev2DataLanguageIntellisensePart> refParts, IParseTO payload, string search, bool addCompleteParts, IList<IIntellisenseResult> result)
        {
            foreach (IDev2DataLanguageIntellisensePart t in refParts)
            {
                var match = t.Name.ToLower();

                if (t.Children != null && t.Children.Count > 0)
                {
                    AddFieldOptions(payload, search, addCompleteParts, match, t, result);
                }
                else
                {
                    if (!match.Contains(search))
                    {
                        continue;
                    }

                    AddFoundItems(payload, t, result);
                }
            }
        }

        static void AddFoundItems(IParseTO payload, IDev2DataLanguageIntellisensePart t, IList<IIntellisenseResult> result)
        {
            if (payload.Parent != null && payload.Parent.Payload.IndexOf(DataListUtil.RecordsetIndexOpeningBracket, StringComparison.Ordinal) >= 0)
            {
                var part = IntellisenseFactory.CreateDataListValidationScalarPart(t.Name, !string.IsNullOrEmpty(t.Description) ? t.Description : " Use row at this index");

                result.Add(IntellisenseFactory.CreateSelectableResult(payload.StartIndex, payload.EndIndex, part, part.Description));
            }
            else
            {
                IDataListVerifyPart part;
                if (t.Name.Contains('(') && t.Name.Contains(')'))
                {
                    part = IntellisenseFactory.CreateDataListValidationScalarPart(t.Name, t.Description);
                }
                else
                {
                    part = IntellisenseFactory.CreateDataListValidationScalarPart(t.Name, t.Description);
                }

                result.Add(IntellisenseFactory.CreateSelectableResult(payload.StartIndex, payload.EndIndex, part, part.Description));
            }
        }

        static void AddFieldOptions(IParseTO payload, string search, bool addCompleteParts, string match, IDev2DataLanguageIntellisensePart t, IList<IIntellisenseResult> result)
        {
            IDataListVerifyPart part;
            if (!addCompleteParts && match.Contains(search))
            {
                part = IntellisenseFactory.CreateDataListValidationScalarPart(t.Name + DataListUtil.RecordsetIndexOpeningBracket, !string.IsNullOrEmpty(t.Description) ? t.Description : " Select a specific row");

                result.Add(IntellisenseFactory.CreateSelectableResult(payload.StartIndex, payload.EndIndex, part, part.Description));

                foreach (IDev2DataLanguageIntellisensePart t1 in t.Children)
                {
                    part = IntellisenseFactory.CreateDataListValidationRecordsetPart(t.Name, t1.Name, !string.IsNullOrEmpty(t1.Description) ? t1.Description : " Input: Use last row, Result: Append new record");
                    result.Add(IntellisenseFactory.CreateSelectableResult(payload.StartIndex, payload.EndIndex, part, part.Description));
                    part = IntellisenseFactory.CreateDataListValidationRecordsetPart(t.Name, t1.Name, !string.IsNullOrEmpty(t1.Description) ? t1.Description : " Use all the rows", "*");
                    result.Add(IntellisenseFactory.CreateSelectableResult(payload.StartIndex, payload.EndIndex, part, part.Description));
                }
                return;
            }
            foreach (IDev2DataLanguageIntellisensePart t1 in t.Children)
            {
                if (t1.Name.Contains(search))
                {
                    part = IntellisenseFactory.CreateDataListValidationRecordsetPart(t.Name, t1.Name, !string.IsNullOrEmpty(t1.Description) ? t1.Description : " Input: Use last row, Result: Append new record");
                    result.Add(IntellisenseFactory.CreateSelectableResult(payload.StartIndex, payload.EndIndex, part, part.Description));
                    part = IntellisenseFactory.CreateDataListValidationRecordsetPart(t.Name, t1.Name, !string.IsNullOrEmpty(t1.Description) ? t1.Description : " Use all the rows", "*");
                    result.Add(IntellisenseFactory.CreateSelectableResult(payload.StartIndex, payload.EndIndex, part, part.Description));
                }
            }
        }

        static void AddIndex(IEnumerable<IDev2DataLanguageIntellisensePart> refParts, IParseTO payload, string search, IList<IIntellisenseResult> result)
        {
            foreach (IDev2DataLanguageIntellisensePart t in refParts)
            {
                if (t.Children == null)
                {
                    var prt = IntellisenseFactory.CreateDataListValidationRecordsetPart(search, "", "", DataListUtil.OpeningSquareBrackets + t.Name + DataListUtil.ClosingSquareBrackets);
                    result.Add(IntellisenseFactory.CreateSelectableResult(payload.StartIndex, payload.EndIndex, prt, prt.Description));
                }
            }
        }

        IList<IIntellisenseResult> ExtractIntellisenseOptions(IParseTO payload, IList<IDev2DataLanguageIntellisensePart> refParts, bool addCompleteParts, IList<IDev2DataLanguageIntellisensePart> additionalParts = null)
        {
            var tmp = new StringBuilder(payload.Payload);
            IList<IIntellisenseResult> result = new List<IIntellisenseResult>();

            if (payload.Payload == string.Empty && payload.HangingOpen)
            {
                ProcessForOnlyOpenRegion(payload, refParts, result);
            }
            else
            {
                if (tmp.Length > 0)
                {
                    ProcessRegion(payload, refParts, addCompleteParts, tmp, result, additionalParts);
                }
                else
                {
                    if (payload.Child == null)
                    {
                        result.Add(IntellisenseFactory.CreateErrorResult(0, 4, null, ErrorResource.VariableIsMissing, enIntellisenseErrorCode.SyntaxError, true));
                        return result;
                    }
                }
            }

            IList<IIntellisenseResult> realResults = new List<IIntellisenseResult>();
            result.ToList().ForEach(r => ProcessResults(realResults, r));
            return result;
        }

        static void ProcessResults(IList<IIntellisenseResult> realResults, IIntellisenseResult r) => _parserHelper.ProcessResults(realResults, r);
        void ProcessRegion(IParseTO payload, IList<IDev2DataLanguageIntellisensePart> refParts, bool addCompleteParts, StringBuilder tmp, IList<IIntellisenseResult> result, IList<IDev2DataLanguageIntellisensePart> additionalParts = null)
        {
            const bool EmptyOk = false;
            if (payload != null)
            {
                var tmpString = tmp.ToString();
                var parts = tmpString.Split('.');
                var search = parts[0].ToLower();
                var rawSearch = search;
                var isRs = false;

                search = RemoveRecordSetBraces(search, ref isRs);
                if (AddFieldResult(payload, result, tmpString, parts, isRs))
                {
                    return;
                }

                if (parts.Length == 1)
                {
                    MatchNonFieldVariables(payload, refParts, addCompleteParts, tmp, result, additionalParts, isRs, rawSearch, search, EmptyOk, parts);
                }
                else if (parts.Length == 2)
                {

                    MatchFieldVariables(payload, refParts, addCompleteParts, result, parts, isRs, rawSearch, search, EmptyOk);
                }
                else
                {
                    var part = IntellisenseFactory.CreateDataListValidationRecordsetPart(parts[0], parts[1]);
                    result.Add(IntellisenseFactory.CreateErrorResult(payload.StartIndex, payload.EndIndex, part, "Invalid Notation - Extra dots detected", enIntellisenseErrorCode.SyntaxError, !payload.HangingOpen));
                }
            }
        }

        void MatchFieldVariables(IParseTO payload, IList<IDev2DataLanguageIntellisensePart> refParts, bool addCompleteParts, IList<IIntellisenseResult> result, string[] parts, bool isRs, string rawSearch, string search, bool emptyOk)
        {
            var tmpTo = new ParseTO { Payload = parts[0], StartIndex = 0, EndIndex = parts[0].Length - 1 };

            var isRecName = isRs && rawSearch.Contains(DataListUtil.RecordsetIndexOpeningBracket) && rawSearch.EndsWith(DataListUtil.RecordsetIndexClosingBracket);

            const string DisplayString = "Recordset";
            if ((!isRecName || parts[1] == string.Empty) && payload.Child == null)
            {
                if (ValidateName(rawSearch, DisplayString, result, out IList<IIntellisenseResult> intellisenseResults))
                {
                    return;
                }
            }
            else
            {
                if (ValidateName(search, DisplayString, result, out IList<IIntellisenseResult> intellisenseResults))
                {
                    return;
                }
            }

            try
            {
                IsValidIndex(tmpTo);
            }
            catch (Dev2DataLanguageParseError e)
            {
                result.Add(AddErrorToResults(isRs, parts[0], e, !payload.HangingOpen));
            }

            var recordsetPart = refParts.FirstOrDefault(c => c.Name.ToLower() == search && c.Children != null);

            var display = parts[0];
            var partName = parts[0];
            var start = display.IndexOf(DataListUtil.RecordsetIndexOpeningBracket, StringComparison.Ordinal);
            if (start >= 0 && recordsetPart == null)
            {
                display = display.Substring(0, start);
                display += "()";
            }

            if (partName.IndexOf(' ') < 0)
            {
                search = ProcessValidPartNameContainingFields(payload, addCompleteParts, result, parts, search, emptyOk, partName, recordsetPart, display);
            }
            else
            {
                var part = IntellisenseFactory.CreateDataListValidationRecordsetPart(parts[0], "." + parts[1], true);
                result.Add(IntellisenseFactory.CreateErrorResult(payload.StartIndex, payload.EndIndex, part, " [[" + display + "]] contains a space, this is an invalid character for a variable name", enIntellisenseErrorCode.SyntaxError, !payload.HangingOpen));
            }
        }

        string ProcessValidPartNameContainingFields(IParseTO payload, bool addCompleteParts, IList<IIntellisenseResult> result, string[] parts, string search, bool emptyOk, string partName, IDev2DataLanguageIntellisensePart recordsetPart, string display)
        {
            if (partName.IndexOf(DataListUtil.RecordsetIndexOpeningBracket, StringComparison.Ordinal) >= 0)
            {
                partName = partName.Substring(0, partName.IndexOf(DataListUtil.RecordsetIndexOpeningBracket, StringComparison.Ordinal));
            }

            if (recordsetPart == null)
            {
                var part = IntellisenseFactory.CreateDataListValidationRecordsetPart(partName, parts[1], "");
                result.Add(IntellisenseFactory.CreateErrorResult(payload.StartIndex, parts[0].Length - 1, part, DataListUtil.OpeningSquareBrackets + display + "]] does not exist in your variable list", enIntellisenseErrorCode.NeitherRecordsetNorFieldFound, !payload.HangingOpen));
            }
            else
            {
                if (recordsetPart.Children != null && recordsetPart.Children.Count > 0 && ProcessFieldsForRecordSet(payload, addCompleteParts, result, parts, out search, out emptyOk, display, recordsetPart, partName))
                {
                    return search;
                }
            }

            if (result.Count == 0 && !emptyOk)
            {
                var part = IntellisenseFactory.CreateDataListValidationRecordsetPart(parts[0], search);
                result.Add(char.IsNumber(search[0]) ? IntellisenseFactory.CreateErrorResult(payload.StartIndex, parts[0].Length - 1, part, "Invalid Expression: Recordset Field [ " + search + " ] starts with a number", enIntellisenseErrorCode.SyntaxError, !payload.HangingOpen) : IntellisenseFactory.CreateErrorResult(parts[0].Length, payload.EndIndex, part, "Recordset Field [ " + search + " ] does not exist for [ " + parts[0] + " ]", enIntellisenseErrorCode.FieldNotFound, !payload.HangingOpen));
            }
            return search;
        }

        bool ProcessFieldsForRecordSet(IParseTO payload, bool addCompleteParts, IList<IIntellisenseResult> result, string[] parts, out string search, out bool emptyOk, string display, IDev2DataLanguageIntellisensePart recordsetPart, string partName)
            => _parserHelper.ProcessFieldsForRecordSet(payload, addCompleteParts, result, parts, out search, out emptyOk, display, recordsetPart, partName);


        void MatchNonFieldVariables(IParseTO payload, IList<IDev2DataLanguageIntellisensePart> refParts, bool addCompleteParts, StringBuilder tmp, IList<IIntellisenseResult> result, IList<IDev2DataLanguageIntellisensePart> additionalParts, bool isRs, string rawSearch, string search, bool emptyOk, string[] parts)
        {
            try
            {
                var isRecName = isRs && rawSearch.Contains(DataListUtil.RecordsetIndexOpeningBracket) && rawSearch.EndsWith(DataListUtil.RecordsetIndexClosingBracket);
                if (!payload.HangingOpen)
                {
                    if (!isRecName && ScalarMatch(result, isRs, rawSearch))
                    {
                        return;
                    }
                    if (RecordsetMatch(result, rawSearch, search))
                    {
                        return;
                    }                    
                }
                if ((rawSearch.Contains(DataListUtil.RecordsetIndexOpeningBracket) && IsValidIndex(payload)) || !rawSearch.Contains(DataListUtil.RecordsetIndexOpeningBracket))
                {
                    foreach (IDev2DataLanguageIntellisensePart t1 in refParts)
                    {
                        emptyOk = MatchVariablesWithNoFields(payload, refParts, addCompleteParts, result, isRs, rawSearch, search, emptyOk, parts, t1);
                    }
                    refParts.ToList().ForEach(pt =>
                    {
                        MatchChildren(payload, result, search, pt);
                    });
                    if (result.Count == 0 && !emptyOk)
                    {
                        FinalEvaluation(payload, tmp, result, additionalParts, isRs);
                    }
                }
            }
            catch (Dev2DataLanguageParseError e)
            {
                result.Add(AddErrorToResults(isRs, parts[0], e, !payload.HangingOpen));
            }
        }

        bool RecordsetMatch(IList<IIntellisenseResult> result, string rawSearch, string search)
        {
            var intellisenseResult = ValidateName(search == string.Empty ? rawSearch : search, "Variable"); //search is empty if there is a scalar value with start open (

            if (intellisenseResult != null)
            {
                result.Add(intellisenseResult);
                return true;
            }
            return false;
        }

        bool ScalarMatch(IList<IIntellisenseResult> result, bool isRs, string rawSearch)
        {
            var intellisenseResult = ValidateName(rawSearch, isRs && !rawSearch.StartsWith(DataListUtil.RecordsetIndexOpeningBracket) ? "Recordset" : "Variable");
            if (intellisenseResult != null)
            {
                result.Add(intellisenseResult);
                return true;
            }
            return false;
        }

        static void FinalEvaluation(IParseTO payload, StringBuilder tmp, IList<IIntellisenseResult> result, IList<IDev2DataLanguageIntellisensePart> additionalParts, bool isRs)
        {
            var display = tmp.ToString().Replace("]", "");
            IDataListVerifyPart part;

            var code = enIntellisenseErrorCode.RecordsetNotFound;
            if (!isRs)
            {
                code = display.IndexOf(' ') >= 0 ? enIntellisenseErrorCode.SyntaxError : enIntellisenseErrorCode.ScalarNotFound;

                part = IntellisenseFactory.CreateDataListValidationScalarPart(display);
            }
            else
            {
                var start = display.IndexOf(DataListUtil.RecordsetIndexOpeningBracket, StringComparison.Ordinal);
                display = display.Substring(0, start);
                display += "()";
                part = IntellisenseFactory.CreateDataListValidationRecordsetPart(display, "");
            }
            if (additionalParts == null)
            {
                result.Add(!display.Contains(' ') ? IntellisenseFactory.CreateErrorResult(payload.StartIndex, payload.EndIndex, part, " [[" + display + "]] does not exist in your variable list", code, !payload.HangingOpen) : IntellisenseFactory.CreateErrorResult(payload.StartIndex, payload.EndIndex, part, " [[" + display + "]] contains a space, this is an invalid character for a variable name", code, !payload.HangingOpen));
            }
            else
            {
                if (!additionalParts.Select(a => a.Name).Contains(display))
                {
                    result.Add(!display.Contains(' ') ? IntellisenseFactory.CreateErrorResult(payload.StartIndex, payload.EndIndex, part, " [[" + display + "]] does not exist in your variable list", code, !payload.HangingOpen) : IntellisenseFactory.CreateErrorResult(payload.StartIndex, payload.EndIndex, part, " [[" + display + "]] contains a space, this is an invalid character for a variable name", code, !payload.HangingOpen));
                }
            }
        }

        static void MatchChildren(IParseTO payload, IList<IIntellisenseResult> result, string search, IDev2DataLanguageIntellisensePart pt)
        {
            pt.Children?.ToList().ForEach(child =>
            {
                var match = child.Name.ToLower();

                if (match.Contains(search))
                {
                    var resultPt = IntellisenseFactory.CreateDataListValidationRecordsetPart(pt.Name, child.Name, pt.Description + " / " + child.Description + " Select this recordset field field");
                    var tmpChild = IntellisenseFactory.CreateSelectableResult(payload.StartIndex, payload.EndIndex, resultPt, resultPt.Description);

                    if (result
                        .ToList()
                        .Find(r => r.Option.DisplayValue == tmpChild.Option.DisplayValue) == null)
                    {
                        result.Add(tmpChild);
                    }
                }
            });
        }

        bool MatchVariablesWithNoFields(IParseTO payload, IList<IDev2DataLanguageIntellisensePart> refParts, bool addCompleteParts, IList<IIntellisenseResult> result, bool isRs, string rawSearch, string search, bool emptyOk, string[] parts, IDev2DataLanguageIntellisensePart t1)
        {
            var match = t1.Name.ToLower();

            if (match.Contains(search) && (match != search))
            {
                if (t1.Children != null && t1.Children.Count > 0)
                {
                    ProcessRecordSetFields(payload, addCompleteParts, result, t1);
                }
                else
                {
                    ProcessNonRecordsetFields(payload, result, t1);
                }
            }
            else if (match == search && isRs)
            {
                emptyOk = rawSearch.Contains(DataListUtil.RecordsetIndexOpeningBracket) && rawSearch.Contains(DataListUtil.RecordsetIndexClosingBracket) ? RecordsetMatch(payload, addCompleteParts, result, rawSearch, search, emptyOk, parts, t1) : ProcessForChild(payload, refParts, result, search, t1);
            }
            else
            {
                if (match == search && !isRs)
                {
                    if (t1.Children != null && t1.Children.Count > 0)
                    {
                        ReturnFieldMatchForRecordSet(payload, result, t1);
                    }
                    else
                    {
                        emptyOk = HandleScalarMatches(payload, addCompleteParts, result, search, t1, match);
                    }
                }
            }
            return emptyOk;
        }

        static bool ProcessForChild(IParseTO payload, IList<IDev2DataLanguageIntellisensePart> refParts, IList<IIntellisenseResult> result, string search, IDev2DataLanguageIntellisensePart t1)
        {
            var emptyOk = false;
            var isHangingChild = payload.Child != null && payload.Child.HangingOpen;

            if (!payload.IsLeaf && !isHangingChild)
            {
                OpenRecordsetItem(payload, result, t1);
            }
            else
            {
                if (payload.Child == null)
                {
                    IDataListVerifyPart prt;
                    foreach (IDev2DataLanguageIntellisensePart t in refParts.Where(t => t.Children == null))
                    {
                        prt = IntellisenseFactory.CreateDataListValidationRecordsetPart(search, "", " / Select a specific row", DataListUtil.OpeningSquareBrackets + t.Name + DataListUtil.ClosingSquareBrackets);
                        result.Add(IntellisenseFactory.CreateSelectableResult(payload.StartIndex, payload.EndIndex, prt, prt.Description));
                    }
                    prt = IntellisenseFactory.CreateDataListValidationRecordsetPart(search, "", " / Reference all rows in the Recordset ", "*");
                    result.Add(IntellisenseFactory.CreateSelectableResult(payload.StartIndex, payload.EndIndex, prt, prt.Description));
                }
                emptyOk = true;
            }
            return emptyOk;
        }

        static bool HandleScalarMatches(IParseTO payload, bool addCompleteParts, IList<IIntellisenseResult> result, string search, IDev2DataLanguageIntellisensePart t1, string match)
        {
            var emptyOk = false;
            if (search != match || (search == match && addCompleteParts))
            {
                if (payload.Parent != null && payload.Parent.Payload.IndexOf(DataListUtil.RecordsetIndexOpeningBracket, StringComparison.Ordinal) >= 0)
                {
                    var p = IntellisenseFactory.CreateDataListValidationScalarPart(t1.Name);
                    result.Add(IntellisenseFactory.CreateSelectableResult(payload.StartIndex, payload.EndIndex, p, " / Select a specific row "));
                }
                else
                {
                    var p = IntellisenseFactory.CreateDataListValidationScalarPart(t1.Name);
                    result.Add(IntellisenseFactory.CreateSelectableResult(payload.StartIndex, payload.EndIndex, p, t1.Description));
                }
            }
            else
            {
                emptyOk = true;
            }
            return emptyOk;
        }

        static void ReturnFieldMatchForRecordSet(IParseTO payload, IList<IIntellisenseResult> result, IDev2DataLanguageIntellisensePart t1)
        {
            var part = IntellisenseFactory.CreateDataListValidationRecordsetPart(t1.Name, "", t1.Description);
            result.Add(IntellisenseFactory.CreateSelectableResult(payload.StartIndex, payload.EndIndex, part, part.Description));
            var children = t1.Children;
            if (children != null)
            {
                foreach (IDev2DataLanguageIntellisensePart t in children)
                {
                    part = IntellisenseFactory.CreateDataListValidationRecordsetPart(t1.Name, t.Name, t.Description + " / Use a field of the Recordset");
                    result.Add(IntellisenseFactory.CreateSelectableResult(payload.StartIndex, payload.EndIndex, part, part.Description));
                }
            }
        }

        static void OpenRecordsetItem(IParseTO payload, IList<IIntellisenseResult> result, IDev2DataLanguageIntellisensePart t1)
            => _recordSetUtil.OpenRecordsetItem(payload, result, t1);


        static bool RecordsetMatch(IParseTO payload, bool addCompleteParts, IList<IIntellisenseResult> result, string rawSearch, string search, bool emptyOk, string[] parts, IDev2DataLanguageIntellisensePart t1)
            => _recordSetUtil.RecordsetMatch(payload, addCompleteParts, result, rawSearch, search, emptyOk, parts, t1);

        static void ProcessNonRecordsetFields(IParseTO payload, IList<IIntellisenseResult> result, IDev2DataLanguageIntellisensePart t1)
            => _recordSetUtil.ProcessNonRecordsetFields(payload, result, t1);


        void ProcessRecordSetFields(IParseTO payload, bool addCompleteParts, IList<IIntellisenseResult> result, IDev2DataLanguageIntellisensePart t1)
            => _recordSetUtil.ProcessRecordSetFields(payload, addCompleteParts, result, t1);


        bool AddFieldResult(IParseTO payload, IList<IIntellisenseResult> result, string tmpString, string[] parts, bool isRs)
            => _parserHelper.AddFieldResult(payload, result, tmpString, parts, isRs);


        static string RemoveRecordSetBraces(string search, ref bool isRs) => _recordSetUtil.RemoveRecordSetBraces(search, ref isRs);

        void ProcessForOnlyOpenRegion(IParseTO payload, IEnumerable<IDev2DataLanguageIntellisensePart> refParts, IList<IIntellisenseResult> result)
        {
            var addAll = !(payload.Parent != null && payload.Parent.IsRecordSet);
            refParts.ToList().ForEach(part =>
            {
                if (part.Children != null && part.Children.Count > 0 && addAll)
                {
                    var tmpPart = IntellisenseFactory.CreateDataListValidationRecordsetPart(part.Name, "", part.Description + " / Select this record set");
                    result.Add(IntellisenseFactory.CreateSelectableResult(payload.StartIndex, payload.StartIndex + 2, tmpPart, tmpPart.Description));
                    part.Children
                        .ToList()
                        .ForEach(child =>
                        {
                            tmpPart = IntellisenseFactory.CreateDataListValidationRecordsetPart(part.Name, child.Name, child.Description + " / Select this record set field");
                            result.Add(IntellisenseFactory.CreateSelectableResult(payload.StartIndex, payload.StartIndex + 2, tmpPart, part.Description + Environment.NewLine + child.Description));
                        });
                }
                else
                {
                    if (part.Children == null)
                    {
                        if (payload.Parent != null && payload.Parent.Payload.IndexOf(DataListUtil.RecordsetIndexOpeningBracket, StringComparison.Ordinal) >= 0 || (part.Name.Contains('(') && part.Name.Contains(')')))
                        {
                            var tmpPart = IntellisenseFactory.CreateDataListValidationRecordsetPart(string.Empty, part.Name, true);
                            result.Add(IntellisenseFactory.CreateSelectableResult(payload.StartIndex, payload.StartIndex + 2, tmpPart, tmpPart.Description + " / Select this record set"));
                        }
                        else
                        {
                            var tmpPart = IntellisenseFactory.CreateDataListValidationScalarPart(part.Name, part.Description + " / Select this variable");
                            result.Add(IntellisenseFactory.CreateSelectableResult(payload.StartIndex, payload.StartIndex + 2, tmpPart, tmpPart.Description));
                        }
                    }
                }
            });
        }

        bool ValidateName(string rawSearch, string displayString, IList<IIntellisenseResult> result, out IList<IIntellisenseResult> intellisenseResults)
            => _parserHelper.ValidateName(rawSearch, displayString, result, out intellisenseResults);

        public IIntellisenseResult ValidateName(string name, string displayString) => _parserHelper.ValidateName(name, displayString);

        IIntellisenseResult AddErrorToResults(bool isRs, string part, Dev2DataLanguageParseError e, bool isOpen) => _parserHelper.AddErrorToResults(isRs, part, e, isOpen);

        bool IsValidIndex(IParseTO to) => _parserHelper.IsValidIndex(to);

        #endregion
    }
}