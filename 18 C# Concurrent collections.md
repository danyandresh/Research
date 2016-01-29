#C# Concurrent collections
##Concurrent counterparts
Can inspect MS source code at http://referencesource.microsoft.com/
 
A method is _atomic_ if other threads can never find it in _half executed_ or _part completed_ states; such method it will either complete succesfully or fail without altering any data it is working with.

A non-atomic method is not thread safe.

_Locking_ only works if it is used consistently across code where shared state is concurrently accessed

_Locking_ is not scalable

Optimistic concurrency - try to update repeatedly.

A race condition occurs when the results are sensitive to the timming of the threads

Concurrent collections offer protection from internal data corruption and race conditions inside method calls, but not between method calls

`System.Collections.Concurrent`

Concurrent collections:

- General purpose
    * `ConcurrentDictionary<TKey, TValue>`

- Producer consumer
    * `ConcurrentQueue<T>`

    * `ConcurrentStack<T>`

    * `ConcurrentBag<T>`
    
    * `IProducerConsumerCollection<T>`
    
- Wrapper for producer/consumers
    * `BlockingCollection<T>`
    
- Partitioners
    * `Partitioner<T>`
    
    * `OrderablePartitioner<T>`
    
    * `Partitioner` - static helper class
    
    * `EnumerablePartitionerOptions`

`Parallel.ForEach` - partitions the iterations between different threads

##`ConcurrentDictionary<TKey, TValue>`
Always succeed:

- `AddOrUpdate()`

- `GetOrAdd()`

Won't throw exception if they fail:

- `TryGetValue()`

- `TryAdd()`

- `TryRemove()` 

- `TryUpdate()`

Each operation should be done in one concurrent collection method call

`IDictionary<TKey, TValue>.Values` - performs poorly in `ConcurrentDictionary`

Break an operation into multiple, simpler ops

Delegates to `AddOrUpdate()` and `GetOrAdd()` execute multiple times.

##`Queue`s, `Stack`s and `Bag`s

###`Queue`

`Peek` and enumeration leaves the items in the queue

`Enqueue` - to add an item

`Dequeue` - to remove the next available item 

###`ConcurrentQueue`

`TryDequeue()` - spinning method to dequeue

`TryPeek()` - spinning method to peek

###`ConcurrentStack`

`Push()` - pushing an item to the stack 

`TryPop()` - spinning method to pop from the head of the stack

###`ConcurrentBag`

`Add` - to add items to the bag

`TryTake` - spinning method to take an item

`ConcurrentBag` has good performance only when the threads that add items to the bag try to remove them in respective order (to avoid _stealing_)

`IProducerConsumerCollection<T>`
    * `TryAdd()`
    * `TryTake()`
    * `Count`
    
Implemented only by concurrent collections

###`BlockingCollection`
wraps other concurrent collections (concretions of `IProducerConsumerCollection<T>` - excepting `ConcurrentDictionary`) + some (blocking) functionality

`Add` - if maximum size is reached this method blocks; it is cancellable

`TryAdd` - allows a timeout

`TryTake` - allows timeout

`Take`  - blocking version of `TryTake`, waiting for an item to become available;
        - throws `InvalidOperationException` when no more items are expected; use `CompleteAdding` to indicate that
        - overload can take cancellation token


##Concurrent collections performance
Concurrent collections are slower than single-threaded courterparts due to thread synchronization ops

Access shared state sparingly to gain performance

Concurrent dictionary uses _fine-grained locking_ to access (lock) different hash buckets from multiple threads and gain performance.

A concurrent dictionary is likely to take collection-wide locks (and hurt performance) when calling:

- `IsEmpty`

- `Count`

- `Clear()`

- `ToArray()`

- `CopyTo()`

- `Values`

- `Keys`

Similarly for `ConcurrentBag`:

- `IsEmpty`

- `Count`

- `ToArray()`

- `CopyTo()`

- `GetEnumerator()`

A concurrent dictionary can be enumerated while being modified, but there is no guarantee of the elements enumerated (if they have been concurrently removed won't be enumerated)

Enumerating concurrent bag, queue or stack produces a snapshot

Standard collections work when multiple threads read data (except when they work with lazily initialized variables)

Concurrent collections are thread safe when it comes to writing data and so are the _immutable_ collections (`System.Collections.Immutable`)





