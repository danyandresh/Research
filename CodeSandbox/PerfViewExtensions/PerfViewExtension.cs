using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Diagnostics.Tracing.Etlx;
using PerfViewExtensibility;
using Address = System.UInt64;

/// <summary>
/// Run C:\Users\Daniel\Downloads\PerfView>PerfView.exe userCommand PerfViewExtensions.AnalyzePrimeFindFrequency PerfViewData.etl.zip
/// to execute this extension
/// </summary>
public class Commands : CommandEnvironment
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="etlFileName"></param>
    public void AnalyzePrimeFindFrequency(string etlFileName)
    {
        LogFile.WriteLine("[AnalyzePrimeFindFrequency with {0}]", etlFileName);
        using (var etlFile = OpenETLFile(etlFileName))
        {
            var events = GetTraceEventsWithProcessFilter(etlFile);

            const int BucketSize = 100;

            List<double> primesPerSecond = new List<double>();

            int numFound = 0;
            DateTime startTime = DateTime.MinValue;
            var stringBuilder = new StringBuilder();
            foreach (var ev in events)
            {
                if (ev.ProviderName == "EtlDemo")
                {
                    if (ev.EventName == "FoundPrime")
                    {
                        if (numFound == 0)
                        {
                            startTime = ev.TimeStamp;
                        }


                        var primeNumber = (long)ev.PayloadByName("primeNumber");
                        if (++numFound == BucketSize)
                        {
                            stringBuilder.AppendLine("new prime");
                            var elapsed = ev.TimeStamp - startTime;
                            var rate = BucketSize / elapsed.TotalSeconds;
                            primesPerSecond.Add(rate);
                            numFound = 0;
                        }

                        stringBuilder.Append(numFound + ", ");
                    }
                }
            }

            var htmlFileName = CreateUniqueCacheFileName("PrimeRateHtmlReport", ".html");
            using (var htmlWriter = File.CreateText(htmlFileName))
            {
                htmlWriter.WriteLine("<h1>Prime discovery rate</h1>");
                htmlWriter.WriteLine("<p>Events: {0}</p>", events.Count());

                htmlWriter.WriteLine("<p>{0}</p>", stringBuilder);

                htmlWriter.WriteLine("<p>Buckets: {0}</p>", primesPerSecond.Count);
                htmlWriter.WriteLine("<p>Bucket Size: {0}</p>", BucketSize);
                htmlWriter.WriteLine("<p>");
                htmlWriter.WriteLine("<table border=\"1\">");

                for (var i = 0; i < primesPerSecond.Count; i++)
                {
                    htmlWriter.WriteLine("<tr><td>{0}</td><td>{1:F2}/sec</td></tr>", i, primesPerSecond[i]);
                }

                htmlWriter.WriteLine("</table>");
            }

            LogFile.WriteLine("[Opening {0}]", htmlFileName);
            OpenHtmlReport(htmlFileName, "Prime Discovery Rate");
        }
    }

    /// <summary>
    /// Gets the TraceEvents list of events from etlFile, applying a process filter if the /process argument is given. 
    /// </summary>
    private TraceEvents GetTraceEventsWithProcessFilter(ETLDataFile etlFile)
    {
        // If the user asked to focus on one process, do so.  
        TraceEvents events;
        if (CommandLineArgs.Process != null)
        {
            var process = etlFile.Processes.LastProcessWithName(CommandLineArgs.Process);
            if (process == null)
                throw new ApplicationException("Could not find process named " + CommandLineArgs.Process);
            events = process.EventsInProcess;
        }
        else
            events = etlFile.TraceLog.Events;           // All events in the process.
        return events;
    }
}