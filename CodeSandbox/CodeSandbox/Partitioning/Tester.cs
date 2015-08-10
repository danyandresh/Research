using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace CodeSandbox.Partitioning
{
    internal class Tester
    {
        [DllImport("Kernel32.dll"), SuppressUnmanagedCodeSecurity]
        public static extern int GetCurrentProcessorNumber();

        public static void Main(string[] args)
        {
            var source = GenerateSource();
            CallMethod(SingleItemListPartitioner, source);
            CallMethod(SingleItemEnumerablePartitioner, source);
        }

        private static OrderablePartitioner<string> SingleItemListPartitioner(IEnumerable<string> source)
        {
            var range = source.ToList();

            var partitioner = new SingleItemListPartitioner<string>(range);

            var partitions = partitioner.GetOrderablePartitions(3);
            // at this stage progressing through each of the partitions would trigger the enumerators to collude and iterate through the source in parallel

            return partitioner;
        }

        private static OrderablePartitioner<string> SingleItemEnumerablePartitioner(IEnumerable<string> source)
        {
            var range = source;

            var partitioner = new SingleItemEnumerablePartitioner<string>(range);

            var partitions = partitioner.GetOrderablePartitions(3);
            // at this stage progressing through each of the partitions would trigger the enumerators to collude and iterate through the source in parallel

            return partitioner;
        }

        private static void CallMethod(Func<IEnumerable<string>, OrderablePartitioner<string>> partitionerMethod, IEnumerable<string> source)
        {
            Console.WriteLine(partitionerMethod.Method.Name + " -> start");

            var partitioner = partitionerMethod(source);

            Parallel.ForEach(partitioner, Print);

            Console.WriteLine(partitionerMethod.Method.Name + " -> end");
            Console.WriteLine();
        }

        private static IEnumerable<string> GenerateSource()
        {
            var range = Enumerable.Range(0, 10).Select(i => "Item_" + i.ToString("00"));

            return range;
        }

        private static void Print(string source, ParallelLoopState state, long index)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId.ToString("00");
            var counter = Stopwatch.StartNew();
            Task.Delay(10).Wait();
            var message = string.Format("Core# {0} Thread# {1} : '{2}'", GetCurrentProcessorNumber(), threadId, source);
            counter.Stop();
            Console.WriteLine("{0} comments: {1} ms {2}", message, counter.ElapsedMilliseconds.ToString("0000"),
                counter.ElapsedMilliseconds > 15
                    ? "! core number could be wrong, operation took longer that thread quantum"
                    : "core number is right, operation took less than thread quantum");
        }
    }
}