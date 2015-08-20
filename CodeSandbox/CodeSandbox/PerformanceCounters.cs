using System;
using System.Diagnostics;

namespace CodeSandbox
{
    internal class PerformanceCounters
    {
        private static void Main(string[] args)
        {
            var counterDataCollection = new CounterCreationDataCollection();

            var bytesPerTx = new CounterCreationData
            {
                CounterType = PerformanceCounterType.AverageCount64,
                CounterName = "BytesPerTransmission"
            };

            counterDataCollection.Add(bytesPerTx);

            var bytesPerTxBase = new CounterCreationData
            {
                CounterType = PerformanceCounterType.AverageBase,
                CounterName = "BytesPerTransmissionBase"
            };

            counterDataCollection.Add(bytesPerTxBase);

            if (!PerformanceCounterCategory.Exists("Network Statistics"))
            {
                PerformanceCounterCategory.Create(
                    "Network Statistics",
                    "Network statistics demo counters",
                    PerformanceCounterCategoryType.SingleInstance,
                    counterDataCollection);
            }

            var networkStatsCounter = new PerformanceCounter("Network Statistics", "BytesPerTransmission", false);
            networkStatsCounter.IncrementBy(1000);
            var counterValue = networkStatsCounter.RawValue;

            Console.WriteLine(counterValue);
        }
    }
}