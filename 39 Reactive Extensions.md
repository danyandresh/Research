#Reactive Extensions
make a query _observable_ by calling `.ToObservable()`

subscribe with `observable.Subscribe()`

```
Observer.Create();

var observable = query.ToObservable(Scheduler.ThreadPool);
observable.ObserveOn(Scheduler.Dispatcher).Subscribe(...)
```

the callstack frames for methods with no params and no result inside of a `Trampoline` are reduced to 1 (to avoid stack overflow)

grammar for observable delegate `OnNext*(OnError|OnCompleted)`

`Disposable.Empty`

`Rx` guarantees not to run two delegates at the same time; the order of execution is guaranteed to be the order of elements

##Scheduler
* `NewThread` - foreground threads

`Immediate` and `CurrentThread` are different

subscriptions cleanup automatically

subscriptions are disposable

subscription can be terminated by calling `Dispose()`

how to compose observers?

`Finally()` ob observable is guaranteed to execute on the observable's scheduler

`Scan()` - aggregate values in the sequence with intermediate results

`Buffer()` - Projects each element of an observable sequence into consecutive non-overlapping buffers which are produced based on element count information.

`Window()` - Projects each element of an observable sequence into consecutive non-overlapping windows which are produced based on element count information.

Difference between Buffers and Windows:

* Buffer returns an observable sequence of lists
* Window returns an observable sequence of observable sequences

##Utility Sequences
`Observable.Empty`

`Observable.Return`

`Observable.Repeat`

`Skip`

`SkipWhile`

`SkipLast` - since it is a sequence (with possibly unknown number of elements) it will buffer a number of elements to be skipped and then call OnNext in pair with generating the next value (thus producing a shift of elements generated and sent to OnNext); when end of sequence the sequence just terminates there

`SkipUntil`

`Concat` - collects observables in order

`Catch` - can chain catches fluently with multiple exception types

`Observable.Throw`

`ZIP` - produces new observable, first input to finish completes processing

using linq extension methods could block further processing

`GroupBy`

```
sequence.GroupBy(number => number % 2)
        .Subscribe(group => group.Count()
                                 .Subscribe(count => Console.WriteLine($"Remainder {group.Key} Count {count}")));
```

Rx is stream oriented

`OnErrorResumeNext`

##OData feed:

- data wrapped in XML
- page (not stream) oriented
- .NET supports OData as a Data Service

`Merge`

`CompositeDisposable`

`Do`

`Timestamp`

`Observable.Timer`

`TimeInterval` - records values between values in sequence

`Throttle` - replaces burst with last value

`Timeout` - prevents subscription from running for too long; should catch `TimeoutException`








