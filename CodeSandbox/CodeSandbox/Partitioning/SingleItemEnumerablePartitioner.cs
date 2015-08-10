using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CodeSandbox.Partitioning
{
    internal class SingleItemEnumerablePartitioner<T> : OrderablePartitioner<T>
    {
        private readonly IEnumerable<T> _source;

        public SingleItemEnumerablePartitioner(IEnumerable<T> source)
            : base(keysOrderedInEachPartition: true, keysOrderedAcrossPartitions: false, keysNormalized: true)
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

            var dynamicPartitioner = new DynamicGenerator(_source.GetEnumerator(), false);

            var result = (from i in Enumerable.Range(0, partitionCount)
                          select dynamicPartitioner.GetEnumerator()).ToList();

            return result;
        }

        public override IEnumerable<KeyValuePair<long, T>> GetOrderableDynamicPartitions()
        {
            return new DynamicGenerator(_source.GetEnumerator(), true);
        }

        private class DynamicGenerator : IEnumerable<KeyValuePair<long, T>>, IDisposable
        {
            private readonly IEnumerator<T> _sharedEnumerator;
            private long _nextAvailablePosition;
            private int _remainingPartitions;
            private bool _disposed;

            public DynamicGenerator(IEnumerator<T> sharedEnumerator, bool requiresDisposal)
            {
                _sharedEnumerator = sharedEnumerator;
                _nextAvailablePosition = -1;
                _remainingPartitions = requiresDisposal ? 1 : 0;
            }

            public IEnumerator<KeyValuePair<long, T>> GetEnumerator()
            {
                Interlocked.Increment(ref _remainingPartitions);
                var result = GetEnumerableCore();

                return result;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Dispose()
            {
                if (_disposed || Interlocked.Decrement(ref _remainingPartitions) != 0)
                {
                    return;
                }

                _disposed = true;
                _sharedEnumerator.Dispose();
            }

            private IEnumerator<KeyValuePair<long, T>> GetEnumerableCore()
            {
                try
                {
                    while (true)
                    {
                        T nextItem;
                        long position;
                        lock (_sharedEnumerator)
                        {
                            if (_sharedEnumerator.MoveNext())
                            {
                                position = _nextAvailablePosition++;
                                nextItem = _sharedEnumerator.Current;
                            }
                            else
                            {
                                yield break;
                            }
                        }

                        yield return new KeyValuePair<long, T>(position, nextItem);
                    }
                }
                finally
                {
                    if (Interlocked.Decrement(ref _remainingPartitions) == 0)
                    {
                        _sharedEnumerator.Dispose();
                    }
                }
            }
        }
    }
}