using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CodeSandbox.Partitioning
{
    internal class SingleItemListPartitioner<T> : OrderablePartitioner<T>
    {
        private readonly IList<T> _source;

        public SingleItemListPartitioner(IList<T> source)
            : base(true, false, true)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            _source = source;
        }

        public override bool SupportsDynamicPartitions { get { return true; } }

        public override IList<IEnumerator<KeyValuePair<long, T>>> GetOrderablePartitions(int partitionCount)
        {
            if (partitionCount < 1)
            {
                throw new ArgumentOutOfRangeException("partitionCount");
            }

            var dynamicPartitioner = GetOrderableDynamicPartitions();
            var result = (from i in Enumerable.Range(0, partitionCount)
                          select dynamicPartitioner.GetEnumerator()).ToList();

            return result;
        }

        public override IEnumerable<KeyValuePair<long, T>> GetOrderableDynamicPartitions()
        {
            return GetOrderableDynamicPartitionsCore(_source, new StrongBox<Int32>(0));
        }

        private IEnumerable<KeyValuePair<long, T>> GetOrderableDynamicPartitionsCore(IList<T> source, StrongBox<int> nextIteration)
        {
            while (true)
            {
                var iteration = Interlocked.Increment(ref nextIteration.Value) - 1;
                if (iteration >= 0 && iteration < source.Count)
                {
                    yield return new KeyValuePair<long, T>(iteration, source[iteration]);
                }
                else
                {
                    yield break;
                }
            }
        }
    }
}