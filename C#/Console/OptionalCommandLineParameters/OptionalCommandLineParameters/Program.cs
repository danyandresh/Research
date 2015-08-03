using OptionalCommandLineParameters;
namespace AP.Management
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;

    //using AP.Utils;

    public class Program
    {
        private static readonly IEnumerable<Tuple<ICollection<Parameter>, Action<IDictionary<string, string>>>>
            SupportedArguments;
        
        static Program()
        {

            //var memorySizeInGiga = MemoryExtensions.AvailableMemoryInBytes();
            //Process.GetCurrentProcess()
            //       .MaxWorkingSet = new IntPtr(memorySizeInGiga);

            Action<IDictionary<string, string>> dummyOperation = (x) => { Console.WriteLine("Operation successful"); };
            var supportedArgs = new List<Tuple<Parameters, Action<IDictionary<string, string>>>>();

            {
                var b=Parameters.CreateOptional();
                b.AddOptional("/buckets=<bucket1|bucket2|bucket3...>", "Fact");
                b.Add("/views=<view>");

                var p = Parameters.Create();
                p.Add("/drilldown",
                      "/host=<host>");
                p.Add(b);
                var drillDownFromToSimplifiedParams = new[]{
                    new Parameter("/drilldown"),
                    new Parameter("/host=<host>"),
                    new Parameter("/buckets=<bucket1|bucket2|bucket3...>",true,"Fact"),
                    new Parameter("/from=<from>"),
                    new Parameter("/to=<to>")
                };

                supportedArgs.Add(Tuple.Create(drillDownFromToSimplifiedParams, dummyOperation));

                var drillDownFromTo = Tuple.Create(
                    new SortedList<int, string>(),
                    new Action<IDictionary<string, string>>(
                        args => TranslationsEngine.DrillDown(args["/host"], args["/from"], args["/to"])));

                drillDownFromTo.Item1.Add(0, "/drilldown");
                drillDownFromTo.Item1.Add(1, "/host=<host>");
                drillDownFromTo.Item1.Add(2, "/from=<from>");
                drillDownFromTo.Item1.Add(3, "/to=<to>");
                supportedArgs.Add(drillDownFromTo);
            }

            var linkProduceMDA = Tuple.Create(
                new SortedList<int, string>(),
                new Action<IDictionary<string, string>>(
                    args => TranslationsEngine.ProduceMDA(args["/host"], args["/bucket"])));

            linkProduceMDA.Item1.Add(0, "/link");
            linkProduceMDA.Item1.Add(1, "/produceMDA");
            linkProduceMDA.Item1.Add(2, "/host=<host>");
            linkProduceMDA.Item1.Add(3, "/bucket=<bucket>");


            var deleteViews = Tuple.Create(
                new SortedList<int, string>(),
                new Action<IDictionary<string, string>>(
                    args => CouchbaseHelper.DeleteViews(args["/host"], args["/bucket"])));
            deleteViews.Item1.Add(0, "/deleteViews");
            deleteViews.Item1.Add(1, "/host=<host>");
            deleteViews.Item1.Add(2, "/bucket=<bucket>");

            var buildGraph = Tuple.Create(
                new SortedList<int, string>(),
                new Action<IDictionary<string, string>>(args => TranslationsEngine.BuildGraph(args["/host"])));
            buildGraph.Item1.Add(0, "/buildGraph");
            buildGraph.Item1.Add(1, "/host=<host>");

            var productTranslationsUpdateLocEnglishText = Tuple.Create(
                new SortedList<int, string>(),
                new Action<IDictionary<string, string>>(
                    args =>
                    TranslationsEngine.UpdateLocTextToEnglish(
                        args["/host"],
                        args["/matchesFile"],
                        args["/matchesSheetName"])));
            productTranslationsUpdateLocEnglishText.Item1.Add(0, "/translations");
            productTranslationsUpdateLocEnglishText.Item1.Add(1, "/updateLocEnglishText");
            productTranslationsUpdateLocEnglishText.Item1.Add(2, "/host=<host>");
            productTranslationsUpdateLocEnglishText.Item1.Add(3, "/matchesFile=<matchesFile>");
            productTranslationsUpdateLocEnglishText.Item1.Add(4, "/matchesSheetName=<matchesSheetName>");

            var productTranslationsUpdateLocIds = Tuple.Create(
                new SortedList<int, string>(),
                new Action<IDictionary<string, string>>(
                    args =>
                    TranslationsEngine.UpdateMstLinks(args["/host"], args["/matchesFile"], args["/matchesSheetName"])));
            productTranslationsUpdateLocIds.Item1.Add(0, "/translations");
            productTranslationsUpdateLocIds.Item1.Add(1, "/updateMSTLinks");
            productTranslationsUpdateLocIds.Item1.Add(2, "/host=<host>");
            productTranslationsUpdateLocIds.Item1.Add(3, "/matchesFile=<matchesFile>");
            productTranslationsUpdateLocIds.Item1.Add(4, "/matchesSheetName=<matchesSheetName>");

            var productTranslationsImportLanguages = Tuple.Create(
                new SortedList<int, string>(),
                new Action<IDictionary<string, string>>(
                    args =>
                    TranslationsEngine.ImportLanguages(args["/host"], args["/mstFile"], args["/mstSheetName"], args["/outputFile"])));
            productTranslationsImportLanguages.Item1.Add(0, "/translations");
            productTranslationsImportLanguages.Item1.Add(1, "/importLanguages");
            productTranslationsImportLanguages.Item1.Add(2, "/host=<host>");
            productTranslationsImportLanguages.Item1.Add(3, "/mstFile=<mstFile>");
            productTranslationsImportLanguages.Item1.Add(4, "/mstSheetName=<mstSheetName>");
            productTranslationsImportLanguages.Item1.Add(5, "/outputFile=<outputFile>");

            var productTextExporter = Tuple.Create(
                new SortedList<int, string>(),
                new Action<IDictionary<string, string>>(
                    args =>
                    TranslationsEngine.ExportProductTextNotInMST(
                        args["/host"],
                        args["/community"],
                        args["/products"],
                        args["/matches"],
                        args["/matchesSheetName"])));
            productTextExporter.Item1.Add(0, "/translations");
            productTextExporter.Item1.Add(1, "/export");
            productTextExporter.Item1.Add(2, "/host=<host>");
            productTextExporter.Item1.Add(3, "/community=<TxnyD.Communities.21.1>");
            productTextExporter.Item1.Add(4, "/products=<product1|product2|product3|...>");
            productTextExporter.Item1.Add(5, "|");
            productTextExporter.Item1.Add(6, "/matches=<matchesFile>");
            productTextExporter.Item1.Add(7, "/matchesSheetName=<matchesSheetName>");

            {
                var exportUSTranslation = Tuple.Create(
                    new SortedList<int, string>(),
                    new Action<IDictionary<string, string>>(
                        args =>
                        TranslationsEngine.ExportUSTranslation(
                            args["/host"],
                            args["/matchesFile"],
                            args["/matchesSheetName"],
                            args["/mstFile"],
                            args["/mstSheetName"],
                            args["/mstDeltaFile"],
                            args["/mstDeltaSheetName"])));
                exportUSTranslation.Item1.Add(0, "/exportLocsUSTranslation");
                exportUSTranslation.Item1.Add(1, "/host=<host>");
                exportUSTranslation.Item1.Add(2, "/matchesFile=<matchesFile>");
                exportUSTranslation.Item1.Add(3, "/matchesSheetName=<matchesSheetName>");
                exportUSTranslation.Item1.Add(4, "/mstFile=<productFile>");
                exportUSTranslation.Item1.Add(5, "/mstSheetName=<mstSheetName>");
                exportUSTranslation.Item1.Add(6, "/mstDeltaFile=<productFile>");
                exportUSTranslation.Item1.Add(7, "/mstDeltaSheetName=<mstSheetName>");
                supportedArgs.Add(exportUSTranslation);
            }

            var exportAllLocs = Tuple.Create(
                new SortedList<int, string>(),
                new Action<IDictionary<string, string>>(
                    args =>
                    TranslationsEngine.ExportLocs(
                        args["/host"])));
            exportAllLocs.Item1.Add(0, "/exportLocs");
            exportAllLocs.Item1.Add(1, "/host=<host>");

            var productDrilldownAndMatchToMSTWithDelta = Tuple.Create(
                new SortedList<int, string>(),
                new Action<IDictionary<string, string>>(
                    args =>
                    TranslationsEngine.MatchProductsToMST(
                        args["/host"],
                        args["/community"],
                        args["/products"],
                        args["/mstFile"],
                        args["/mstSheetName"],
                        args["/mstDeltaFile"],
                        args["/mstDeltaSheetName"])));
            productDrilldownAndMatchToMSTWithDelta.Item1.Add(0, "/match");
            productDrilldownAndMatchToMSTWithDelta.Item1.Add(1, "/toMST");
            productDrilldownAndMatchToMSTWithDelta.Item1.Add(2, "/host=<host>");
            productDrilldownAndMatchToMSTWithDelta.Item1.Add(3, "/community=<TxnyD.Communities.21.1>");
            productDrilldownAndMatchToMSTWithDelta.Item1.Add(4, "/products=<product1|product2|product3|...>");
            productDrilldownAndMatchToMSTWithDelta.Item1.Add(5, "/mstFile=<productFile>");
            productDrilldownAndMatchToMSTWithDelta.Item1.Add(6, "/mstSheetName=<mstSheetName>");
            productDrilldownAndMatchToMSTWithDelta.Item1.Add(7, "/mstDeltaFile=<productFile>");
            productDrilldownAndMatchToMSTWithDelta.Item1.Add(8, "/mstDeltaSheetName=<mstSheetName>");

            var productDrilldownAndMatchToMST = Tuple.Create(
                new SortedList<int, string>(),
                new Action<IDictionary<string, string>>(
                    args =>
                    TranslationsEngine.MatchProductsToMST(
                        args["/host"],
                        args["/community"],
                        args["/products"],
                        args["/mstFile"],
                        args["/mstSheetName"])));
            productDrilldownAndMatchToMST.Item1.Add(0, "/match");
            productDrilldownAndMatchToMST.Item1.Add(1, "/toMST");
            productDrilldownAndMatchToMST.Item1.Add(2, "/host=<host>");
            productDrilldownAndMatchToMST.Item1.Add(3, "/community=<TxnyD.Communities.21.1>");
            productDrilldownAndMatchToMST.Item1.Add(4, "/products=<product1|product2|product3|...>");
            productDrilldownAndMatchToMST.Item1.Add(5, "/mstFile=<productFile>");
            productDrilldownAndMatchToMST.Item1.Add(6, "/mstSheetName=<mstSheetName>");



            var mstDiff = Tuple.Create(
                new SortedList<int, string>(),
                new Action<IDictionary<string, string>>(
                    args => TranslationsEngine.MstDiff(args["/mstFileUK"], args["/mstFileUS"])));

            mstDiff.Item1.Add(0, "/mst");
            mstDiff.Item1.Add(1, "/intersect");
            mstDiff.Item1.Add(2, "/mstFileUK=<productFile>");
            mstDiff.Item1.Add(3, "/mstFileUS=<productFile>");

            var matchDrilledLocsToMST = Tuple.Create(
                new SortedList<int, string>(),
                new Action<IDictionary<string, string>>(
                    args =>
                    TranslationsEngine.IntersectProductsWithMST(
                        args["/host"],
                        args["/productFile"],
                        args["/productSheetName"],
                        args["/mstFile"],
                        args["/mstSheetName"])));

            matchDrilledLocsToMST.Item1.Add(0, "/product");
            matchDrilledLocsToMST.Item1.Add(1, "/intersect");
            matchDrilledLocsToMST.Item1.Add(2, "/withMST");
            matchDrilledLocsToMST.Item1.Add(3, "/host=<host>");
            matchDrilledLocsToMST.Item1.Add(4, "/productFile=<productFile>");
            matchDrilledLocsToMST.Item1.Add(5, "/productSheetName=<productSheetName>");
            matchDrilledLocsToMST.Item1.Add(6, "/mstFile=<productFile>");
            matchDrilledLocsToMST.Item1.Add(7, "/mstSheetName=<mstSheetName>");

            var productDrilldownByCommunity = Tuple.Create(
                new SortedList<int, string>(),
                new Action<IDictionary<string, string>>(
                    args => TranslationsEngine.ExtractProducts(args["/host"], args["/community"])));

            productDrilldownByCommunity.Item1.Add(0, "/product");
            productDrilldownByCommunity.Item1.Add(1, "/drilldown");
            productDrilldownByCommunity.Item1.Add(2, "/host=<host>");
            productDrilldownByCommunity.Item1.Add(3, "/community=<TxnyD.Communities.21.1>");

            var productDrilldownToKeywords = Tuple.Create(
                new SortedList<int, string>(),
                new Action<IDictionary<string, string>>(
                    args => TranslationsEngine.ExtractProducts(args["/host"], args["/community"], args["/products"], args["/keywords"])));

            productDrilldownToKeywords.Item1.Add(0, "/product");
            productDrilldownToKeywords.Item1.Add(1, "/drilldownToKeywords");
            productDrilldownToKeywords.Item1.Add(2, "/host=<host>");
            productDrilldownToKeywords.Item1.Add(3, "/community=<TxnyD.Communities.21.1>");
            productDrilldownToKeywords.Item1.Add(4, "/products=<product1|product2|product3|...>");
            productDrilldownToKeywords.Item1.Add(5, "/keywords=<keyword1|keyqord2|keyword3|...>");

            var productDrilldownByProductList = Tuple.Create(
                new SortedList<int, string>(),
                new Action<IDictionary<string, string>>(
                    args => TranslationsEngine.ExtractProducts(args["/host"], args["/community"], args["/products"])));

            productDrilldownByProductList.Item1.Add(0, "/product");
            productDrilldownByProductList.Item1.Add(1, "/drilldown");
            productDrilldownByProductList.Item1.Add(2, "/host=<host>");
            productDrilldownByProductList.Item1.Add(3, "/community=<TxnyD.Communities.21.1>");
            productDrilldownByProductList.Item1.Add(4, "/products=<product1|product2|product3|...>");

            var linkProduceNqd = Tuple.Create(
                new SortedList<int, string>(),
                new Action<IDictionary<string, string>>(
                    args => TranslationsEngine.ProduceNqds(args["/host"], args["/bucket"])));

            linkProduceNqd.Item1.Add(0, "/link");
            linkProduceNqd.Item1.Add(1, "/produceNQD");
            linkProduceNqd.Item1.Add(2, "/host=<host>");
            linkProduceNqd.Item1.Add(3, "/bucket=<bucket>");

            var linkMatch = Tuple.Create(
                new SortedList<int, string>(),
                new Action<IDictionary<string, string>>(
                    args => TranslationsEngine.MatchNqds(args["/nqdFile"], args["/mstFile"])));

            linkMatch.Item1.Add(0, "/link");
            linkMatch.Item1.Add(1, "/match");
            linkMatch.Item1.Add(2, "/nqdFile=<file path>");
            linkMatch.Item1.Add(3, "/mstFile=<file path>");



            supportedArgs.AddRange(
                new[]
                    {
                        buildGraph, productDrilldownAndMatchToMST, linkProduceNqd, linkMatch, productDrilldownByCommunity,
                        productDrilldownByProductList, matchDrilledLocsToMST, mstDiff, productTranslationsUpdateLocIds,
                        deleteViews, productTranslationsUpdateLocEnglishText, linkProduceMDA, productTextExporter,
                        productDrilldownToKeywords, productDrilldownAndMatchToMSTWithDelta, exportAllLocs,productTranslationsImportLanguages
                    });

            SupportedArguments = supportedArgs;
        }

        public static void Main(string[] args)
        {
            var argumentsValidationResult = ValidateArguments(args);

            if (!argumentsValidationResult.IsValid)
            {
                Console.WriteLine(argumentsValidationResult.Message);
                return;
            }

            argumentsValidationResult.ExecuteAttachedAction();
        }

        private static ArgumentsValidationResult ValidateArguments(string[] args)
        {
            var matchingArguments = (from x in SupportedArguments
                                     let match = MatchArgument(x.Item1, args)
                                     where match.IsMatch
                                     select new { Arguments = match.InspectedArgParams, Action = x.Item2 }).ToArray();

            if (matchingArguments.Length < 1)
            {
                return ArgumentsValidationResult.CreateInvalid(SupportedArguments);
            }

            if (matchingArguments.Length > 1)
            {
                throw new ArgumentException("Too many argument matching: ", string.Join(" ", args));
            }

            return ArgumentsValidationResult.CreateValid(
                matchingArguments.Single().Arguments,
                matchingArguments.Single().Action);
        }

        private static ArgumentMatchResult MatchArgument(SortedList<int, string> supportedArg, string[] inputArg)
        {
            var inspectedArgParams = new Dictionary<string, string>();
            if (supportedArg.Count != inputArg.Length)
            {
                return new ArgumentMatchResult(false, supportedArg, inspectedArgParams);
            }

            var i = 0;

            for (; i < supportedArg.Count; i++)
            {
                //TODO: Needs revisiting
                var regexPattern = Regex.Replace(supportedArg[i], "=<[^>]+>", "=.+");
                regexPattern = string.Join("=", regexPattern.Split('=').Select(x => string.Format("({0})", x)));

                var argumentMatchesPattern = Regex.Match(inputArg[i], regexPattern);
                if (!argumentMatchesPattern.Success)
                {
                    break;
                }

                if (argumentMatchesPattern.Groups.Count > 2)
                {
                    inspectedArgParams.Add(
                        argumentMatchesPattern.Groups[1].Value,
                        argumentMatchesPattern.Groups[2].Value);
                }
            }

            return new ArgumentMatchResult(i == supportedArg.Count, supportedArg, inspectedArgParams);
        }

        private class ArgumentMatchResult
        {
            public ArgumentMatchResult(
                bool isMatch,
                SortedList<int, string> supportedArg,
                IDictionary<string, string> inspectedArgParams)
            {
                IsMatch = isMatch;
                SupportedArg = supportedArg;
                InspectedArgParams = inspectedArgParams;
            }

            public bool IsMatch { get; private set; }

            public SortedList<int, string> SupportedArg { get; set; }

            public IDictionary<string, string> InspectedArgParams { get; set; }
        }

        private class ArgumentsValidationResult
        {
            public bool IsValid { get; private set; }

            public Action<IDictionary<string, string>> AttachedAction { get; private set; }

            public IDictionary<string, string> Arguments { get; private set; }

            public string Message { get; private set; }

            public void ExecuteAttachedAction()
            {
                AttachedAction(Arguments);
            }

            public static ArgumentsValidationResult CreateInvalid(
                IEnumerable<Tuple<SortedList<int, string>, Action<IDictionary<string, string>>>> supportedArguments)
            {
                var buildMessage = "Available commands" + Environment.NewLine + "\t"
                                   + string.Join(
                                       Environment.NewLine + "\t",
                                       supportedArguments.Select(x => string.Join(" ", x.Item1.Select(y => y.Value))));

                return new ArgumentsValidationResult { IsValid = false, Message = buildMessage };
            }

            public static ArgumentsValidationResult CreateValid(
                IDictionary<string, string> arguments,
                Action<IDictionary<string, string>> action)
            {
                return new ArgumentsValidationResult { IsValid = true, Arguments = arguments, AttachedAction = action };
            }
        }
    }
}