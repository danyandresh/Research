using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CodeSandbox.Partitioning
{
    internal class Tester
    {
        public static void Main(string[] args)
        {
            SingleItemListPartitioner();
        }

        private static void SingleItemListPartitioner()
        {
            var range = Enumerable.Range(0, 10).Select(i => "Item_" + i.ToString("00")).ToList();

            var partitioner = new SingleItemListPartitioner<string>(range);

            var partitions = partitioner.GetOrderablePartitions(3);
            // at this stage progressing through each of the partitions would trigger the enumerators to collude and iterate through the source in parallel

            Parallel.ForEach(partitioner, (source, state, index) =>
            {
                Task.Delay(100);
                Console.WriteLine("#{0}: {1}", Thread.CurrentThread.ManagedThreadId, source);
            });
        }
    }
}