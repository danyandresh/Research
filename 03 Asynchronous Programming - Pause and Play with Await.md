##[Asynchronous Programming - Pause and Play with Await](https://msdn.microsoft.com/en-us/magazine/hh456403.aspx)
###Sequential composition:
Building the program logic using control structures like: decision structures (`if`, `switch`), looping structures (`for`, `foreach`, `do/while`) and control transfer statements (`continue`, `throw` and `goto`)

###Continuous execution
In `C#` methods and blocks of code execute to completion when a _thread of control_ started; threads can be blocked, while waiting for a resource to become available or while waiting for another thread to complete.
###Asynchronous programming
Method's execution can be paused, freeing the execution thread for other operations.
####How is it possible? Compiler rewriting
Methods are rewritten by the compiler to employ `statemachine`s - that keeps the track of current context - which have two states: _running_ or _paused_. `await` is setting up the context and preparing the machine with all that it needs so when the awaited operation completes the execution can be resumed.
####Task builders
The compiler builds methods that return a `Task` or `Task<T>` (`System.Threading.Tasks`) using Task builder from the framework (`System.Runtime.CompilerServices`) 

Example of structure compiler generated:

```c#
Task<T> MethodName()
{
    var __builder = new AsyncTaskMethodBuilder<T>();
    ...
    Action __moveNext = delegate
    {
        try
	    {
	        ...
	        return;
	        ...
	        __builder.SetException(exception);
	    }
	    catch (Exception exception)
	    {
	        __builder.SetException(exception);
	    }
    };
  
    __moveNext();

    return __builder.Task;
}
```

Where the structure of `AsyncTaskMethodBuilder<T>` is similar to
```c#
public class AsyncTaskMethodBuilder<TResult>
{
  public Task<TResult> Task { get; }
  public void SetResult(TResult result);
  public void SetException(Exception exception);
}
```

This is very similar to using the `TaskCompletionSource` from _TPL_

####Awaitables and Awaiters

`C#` can await to any object that has a `GetAwaiter` method defined that returns an `awaiter` (`TaskAwaiter<T>`).
```c#
public struct TaskAwaiter<TResult>
{
    public bool IsCompleted { get; }
	public void OnCompleted(Action continuation);
	public TResult GetResult();
}
```

So the `awaiter` is used like this
```c#
__awaiter1 = MethodAsync().GetAwaiter();
if(!__awaiter1.IsCompleted) 
{
    ... // Prepare for resumption at Resume1
	__awaiter1.OnCompleted(__moveNext);
	
	return
}

Resume1:
    ... _awaiter1.GetResult() ...	
```

`Task.Yield` Is a utility in TPL that helps circumventing the compiler optimization of serving the result directly if it is available.

####The State Machine
Takes care of capturing variables values for later reference, current state of the operation; it uses a `switch` statement for the current state that can resume execution.

```c#
Task<T> MethodName()
{
    var __builder = new AsyncTaskMethodBuilder<T>();
    int __state = 0;
    Action __moveNext = null;
    TaskAwaiter<TResult> __awaiter1;
    Action __moveNext = delegate
    {
        try
	    {
		
		    if (__state == 1)
			{
			    goto Resume1;
			}
			
			__awaiter = MethodAsync().GetAwaiter();
			if (!__awaiter.IsCompleted)
			{
				_state = 1;
				__awaiter.OnCompleted(__moveNext);
				
				return;
			}
			
			Resume1:
			__builder.SetResult(__awaiter.GetResult());
		}
		catch (Exception exception)
		{
			__builder.SetException(ex);			
		}
    };
  
    __moveNext();

    return __builder.Task;
}
```

####Compiler rewriting issues

* **Goto statements** cannot be used when buried into a nested structure (`try`); this is solved by jumping at the beginning of the structure, enter it normally, switch and jump again
* **Finally blocks** are not to be executed when returning out of a _resumption_ delegate; they should be executed when the _real_ return happens - normally achieved with a boolean flag indicating whether the `finally` should be executed or not
* **Evaluation order** must be preserved by evaluating everything that is before `await`, store the results and retrieve them after `await`

#####`await`s limitation

Can't be used inside a `try/catch` or `finally` block for the exception context can't be properly reestablished.

####Task awaiter

The execution scheduling of a resumption delegate is down to the awaiter used by the compiler; similarly, `task`s execution is leveraging the pluggable _scheduling context_.

_Scheduling context_ is a thread-affine concept (each thread has at most one) that represents _where_ the execution was left off and it should continue (for _awaited_ tasks).

An asynchronous method that awaits a task should:

* On suspension: ask the current thread for the scheduling context
* On resumption: schedule the resumption delegate on the scheduling context (captured above)

#####Scheduling Context

It is important that the continuation is executed on the right _scheduling context_ so code that deals with UI elements doesn't have _suprising_ behaviour after an `await`able operation.
The following concepts play the role of a _scheduling context_, in order:

1. Thread's _SynchronizationContext_ if any;
2. If not, then the thread's _TaskScheduler_ (a similar concept introduced in TPL) if any;
3. Default _ThreadScheduler_ - resumptions are scheduled to the thread pool.

Default _ThreadScheduler_ is common to all threads from the standard thread pool, thus making no difference on what thread the resumption is executed.

###### Optimizing Scheduling Context switching

In order to prevent useless context switching the awaited task can be wrapped in a non-task awaitable that turns off the scheduling back to the originating scheduling context and runs the continuation on the thread completing the task; after the completion of the resume block, on exiting to the method the scheduling context is switched back to original, unless otherwise specified.

See [CodeSandbox/AsyncWpfApplication](CodeSandbox/AsyncWpfApplication/MainWindow.xaml.cs) for an example.