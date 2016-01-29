#Functional programming with C#
##Better code with Functional programming

Clean code:
- keep functions small
- DRY
- do one thing
- avoid side effects
- functions should accept no more than 3 params

> OO makes code understandable by _encapsulating moving parts_
> FP makes code understandable by _minimizing moving parts_
> - Michael Feathers

###What is FP?
Functional programming is a paradigm which concentrates on computing results
rather than on performing actions

1. FP primary goal is to control side-effects

###Side-effect
is any accompanying or consequential and usually detrimental effect

Functional purity:
- purely functional languages
- impure (C#)

2. Expression based

Favour expression over statements,

expressions are naturally composable

3. Treat functions as data

Enable higher-order functions to
* accept other functions as arguments
* return other functions as result

##Expressions
Enforce immutability (both external and internal) to ease the expression-based goal

C# is statement based but has support for functional approach too

Use expressions over the statements whenever possible

```c#
public static MyType Instance
{
    get
    {
        return _instance ?? (_instance = new MyType());
    }
}
```

```c#
//C# 6 expression bodied

public static MyType Instance => _instance ?? (_instance = new MyType());
```
##Functional thinking
MulticastDelegate is an abstract class which represents one or more methods to be invoked

Initially the delegation was slow, now anonymous delegates outperform normal method calls in some cases.

* Delegates are .NET mechanism for treating functions as data
* Delegates are derived from MulticastDelegate
* Generic delegate types provide a common vocabulary for delegation
* Lambda expressions are convenient inline methods

##Going with the flow

Pipelining -> pipe the output of an operation as input for the next

Method chaining -> close to the definition of pipelining, architectural pattern which must be deliberately designed into types being chained

Method chaining tends to break down when working across disparate types

###Disposable Using
```c#
public static class Disposable
{
    public static TResult Using<TDisposable, TResult>
                            (Func<TDisposable> factory,
                             Func<TDisposable, TResult> fn)
                            where TDisposable : IDisposable
    {
        using (var disposable = factory())
        {
            return fn(disposable);
        }
    }
}
```
 
###Map
Tranforming an object into another is _mapping_ in functional terms
```c#
public static TResult Map<TSource, TResult>(
                        this TSource @this,
                        Func<TSource, TResult> fn) => fn(@this);
```

###Tee
Chain execution of methods that have side effects (like logging, populating an array, etc.)
```c#
public static T Tee<T>(
                    this T @this,
                    Action<T> act)
{
    act(@this);
    
    return @this;
}
```
###Partial function application
Partial function application - tying parameters to a function call, yet deferring its execution
Currying - breaking down function into multiple functions

* Pipelining allows data to flow between functions
* Method chaining is the OO version of pipelining
* Types must be designed with chaining in mind
* Higher-order extension methods enable global method chaining
* Partial function application improves composability

1. C#'s built-in functional features improve:
    - Predictability
    - Maintainability
    - Testability
2. Controlling side effects through immutability and scope
3. Preferring expressions over statements
4. Pipelining through higher-order extension methods



 