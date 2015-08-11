### Tasks

> The Task Parallel Library (TPL) is based on the concept of a task, which represents an asynchronous operation. In some ways, a task resembles a thread or ThreadPool work item, but at a higher level of abstraction. The term task parallelism refers to one or more independent tasks running concurrently

[TPL on MSDN](https://msdn.microsoft.com/en-us/library/dd537609.aspx)

#### `TaskCompletionSource`

> Represents the producer side of a Task<TResult> unbound to a delegate, providing access to the consumer side through the Task property

Very handy when _faking_ a task to return a value when a chain of tasks are completed

```csharp
public static Task<Stream> BufferStream(this Stream sourceStream, int chunkSize = 1024)
{
	var result = new TaskCompletionSource<Stream>();

	var buffer = new byte[chunkSize];
	var localStream = new MemoryStream
	{
		Capacity = chunkSize
	};

	ReadToBuffer(buffer, sourceStream, localStream, result);

	return result.Task;
}
```

In this example the result `tcs` is apssed down the recursive methods fro reading from source stream and writing to the local _buffering_ stream. It isn't necessarily good practice (to buffer a stream on the server) but can be used to cache locally resources that are repeatedly needed.

See the rest of the implementation [here](CodeSandbox/CodeSandbox/Parallelization.cs)

#### `Task<T>.Factory.FromAsync` to convert APM model to Tasks

```c#
private static void ReadToDestinationBuffer(byte[] buffer, Stream sourceStream, List<byte> buffers, TaskCompletionSource<IEnumerable<byte>> taskCompletionSource)
{
	var t = Task<int>.Factory.FromAsync(
		sourceStream.BeginRead,
		sourceStream.EndRead,
		buffer,
		0,
		buffer.Length,
		null);

	t.ContinueWith(
		readTask => WriteToBuffer(readTask, buffer, sourceStream, buffers, taskCompletionSource));
}
```

#### `Interlocked` **atomic** operations

* `Add` - adds two integers, replacing the first one with the sum and returning that as the result;
* `CompareExchange` - `CompareExchange(ref location, newValue, comparand)` -  compares `location` with `comparand` and if they are equal `location` is replaced with `newValue`; returns the old value
* `Increment` - adds `1` to the value and returns the result
* `Decrement` - substracts `1` from the value and returns the result
* `Exchange` - replaces `location` with a new value and returns the old value

[ECMA C# and Common Language Infrastructure Standards](https://msdn.microsoft.com/en-US/vstudio/aa569283.aspx)

#### Locking

##### `Monitor`/`lock` 

Never lock on:

- `MarshalByRefObject`, a proxy that doesn't protect the underlying source
- `string` which is shared unpredictably
- `value type` as they trigger a boxing each time lock is called, precluding synchronization entirely

##### Asynchronous locks: `SehmaphoreSlim` 

- returns a continuation task to execute once the semaphore permits it

##### `SpinLock`

- use if a lock is short-lived, it will spin a loop until the contention is done
- `Monitor` is a better option as it _spins_ first then enters `wait`

##### `Slim` synchronization 

- prefer the `Slim` versions of synchronization objects, they are implemented to _sping_ first then _wait_ (entering Kernel transition which is much slower)
