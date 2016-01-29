#C# Programming paradigms
##C# and LINQ
- Linq provides general purpose query facilities

`ISequence<T>`

`IEnumerable<T>`
- `GetEnumerator()` - being the only method makes IEnumerable very versatile, as SRP and Interface Segregation principles are respected
- extension through extension methods

###Expressions
- Compiler builds abstract syntax trees (and not IL)
- It's up to a LINQ provider to turn this data structure into component specific code (e.g. T-SQL)

Comprehension syntax is transformed into extension methods call

##C# and the DLR
DLR Allows Ruby and Python code to be execute on .NET Runtime

A _dynamic_ language will allow change of code and types at runtime; it will not perform type checking.

`dynamic` - dynamically typed variables in C#, compile time checks are turned off.

A _call site_ is the place where an operation on a dynamic object is performed

###ExpandoObject

`System.Dynamic.ExpandoObject` defines a dynamic type that can arbitrarily add properties and methods.

```c#
dynamic expando = new ExpandoObject();

expando.Property1 = "First property";
expando.Method1() = ()=>Console.WriteLine("Method 1 executed");
```

Expando of XML document
```c#
public static dynamic AsExpando(this XDocument document)
{
    return CreateExpando(document.Root);
}

private static dynamic CreateExpando(XElement element)
{
    var result = new ExpandoObject() as IDictionary<string, string>;
    
    if(element.Elements().Any(e => e.HasElements))
    {
        var childElements = element.Elements().Select(e => CreateExpando(e)).ToList();
        
        result.Add(element.Name.ToString(), childElements);
    }
    else
    {
        foreach(var leaf in element.Elements())
        {
            result.Add(leaf.Name.ToString(), leaf.Value);
        }
    }
    
    return result;
}
```

###DynamicObject

> Provides a base class for specifying dynamic behavior at run time

`TryGetMember`

http://ironruby.net

##Object Oriented Programming
Encapsulation

Plymorphysm

Inheritance (limited, only when a type **nuaturally** inherits a _full_ set of attributes)

Dependencies should _point in_ not _out_ (i.e. types expose their dependencies in constructor as opposed to trying to reach out for them)

Separation between deciding and doing

##Functional programming with C#
Small abstractions are very versatile; they are easy to understand, maintain and reuse.

Functional programming: combining little abstrations to build (bigger) systems

###Lazy evaluation
Code that execute only when the result is needed

`yield` - state machine that facilitates lazily execution of sequences.

Lazy functions: functions that execute the least amount of work needed to produce the proper results

###Partial Application and Currying

* partial function application: converts a function with N parameters to a function with N - 1 params, 
* currying decomposes a function into functions taking a single parameter
```c#
public static Func<TResult> Partial<TParam1, TResult>
(this Func<TParam1, TResult> func, TParam1 parameter)
{
        return ()=> func(parameter);
}

public static Func<TParam1, Func<TResult>> Curry<TParam1, TResult>
(this Func<TParam1, TResult> func)
{
        return parameter => ()=> func(parameter);
}
```

TPL is an example of functional API

##Crafting C# code
[microsoft design guidelines for developing class libraries, Framework Design Guidelines](https://msdn.microsoft.com/en-us/library/ms229042.aspx)

10. Avoid regions; they hide ugly code
9. Use exceptions for errors; ... instead of status codes or booleans, but not for control flow
8. Avoid boolean parameters to a function
7. Avoid too many parameters; refactor to use an objects instead of the list of params, as there may be validation or other kind of functionality required too
6. Setup build to treat compiler Warnings as Errors
5. Encapsulate complex expressions
4. Try to avoid multiple exits; 
3. Try to avoid comments; meaningless noise; try to get code to reveal its intention so any comment would be superfluous
2. Keep methods short; 1-10 is safe, 10-20 so-so, over 20 too big
1. Keep classes small; use roles, separate responsibilities, delegate




 