### Partitioning

> Partitioning is the act of splitting a data set to be processed in parallel by multiple logical processors. It is a fundamental aspect of parallel computing, as without partitioning everything would execute serially.

[From Stephen Toub, member of Parallel Computing Platform team at Microsoft](http://www.drdobbs.com/windows/custom-parallel-partitioning-with-net-4/224600406)

#### Parallel.ForEach (System.Threading.Tasks)

#### `Partitioner<TSource>`

```c#
public abstract class Partitioner<TSource>
{   
    protected Partitioner();
 
    public abstract IList<IEnumerator<TSource>> GetPartitions(
        int partitionCount);
 
    public virtual bool SupportsDynamicPartitions { get; }
    public virtual IEnumerable<TSource> GetDynamicPartitions();
}
```

#### `OrderablePartitioner<TSource>`

```c#
public abstract class OrderablePartitioner<TSource> : Partitioner<TSource>
{
    protected OrderablePartitioner(
       bool keysOrderedInEachPartition, bool keysOrderedAcrossPartitions, 
       bool keysNormalized);
 
    public abstract IList<IEnumerator<KeyValuePair<long, TSource>>> 
        GetOrderablePartitions(int partitionCount);
    public virtual IEnumerable<KeyValuePair<long, TSource>> 
        GetOrderableDynamicPartitions();
 
    public bool KeysNormalized { get; }
    public bool KeysOrderedAcrossPartitions { get; }
    public bool KeysOrderedInEachPartition { get; }
}
```

#### [`SingleItemListPartitioner`](CodeSandbox/CodeSandbox/Partitioning/SingleItemListPartitioner.cs)

Key to an [OpenMP](http://openmp.org/wp/) `dynamic(1)` like implementation of the `OrderablePartitioner` is `GetOrderableDynamicPartitionsCore` defined below:

```csharp
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
```

This is leveraging `StrongBox` to create a strong reference to the current _iteration_ **value** - so it can be safely incremented from different threads.

Note: this is a load balanced partitioner and calling .ToList() on any individual partition would cause the other partitions to be become empty.

#### [`SingleItemEnumerablePartitioner`](CodeSandbox/CodeSandbox/Partitioning/SingleItemEnumerablePartitioner.cs)

Creating a partitioner on an enumerable source (that doesn't offer the luxury of _indexing_) is a bit trickier, especially when it come to disposing the enumerators

Key to such implementation is `DynamicGenerator.GetEnumeratorCore()` listed below:
```c#
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
```

As with `list` based implementation above, calling `ToList()` on any partition would _dry_ the rest of partitions (for it is a balanced partitioning)

#### More info

[Samples for Parallel Programming with the .NET Framework](http://code.msdn.microsoft.com/ParExtSamples)

[Parallel Computing](http://msdn.com/concurrency)

