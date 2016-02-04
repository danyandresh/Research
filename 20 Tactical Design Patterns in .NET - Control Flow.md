#Tactical Design Patterns in .NET: Control Flow
When control flow is done right:
- code is simpler and shorter
- methods are shorter
- coordination between classes is easier to follow
- classes are simpler and shorter when flow decisions are made differently

###Complexity
number of mutually independent execution paths

code complexity is not the same as asymptotic complexity

####Cyclomatic complexity
is the number of linearly independent parts through the control flow graph

control flow graph is a directed graph of nodes, can be copied from the control flow diagram

`number of edges` - `number of nodes` + 2

should have a test for each path through the control flow graph (**path coverage** testing strategy)

**branch coverage** should have tests for all branches, timpically lower than the cyclomatic complexity

####NDepend
for static code analysis

####Removing optional arguments
adds clarity and simplifies the design by splitting the method into two mutually disjoined methods. this removes the uncertainty from method signatures. 

simplified control flow and removal of potential bugs

###Null references

carry no value

####Issues with nulls
- too much branching on nulls
- missing information when null is returned

- `Null Object` pattern will eliminate the branching
- `Special Case` pattern will allow additional information in special cases
- `Option<T>` functional type and map reduce pattern

##Refactoring to get rid of nulls
###Null Object pattern
Null objects have absolutely no behaviour

Null objects should be singletons

###Special Case pattern

objects that add behaviour that null objects cannot cary

* high level modules should not depend on low level modules
* both should depend on abstractions
* abstractions should not depend on details
* details should depend on abstractions

should create null and special case objects through factory

Null object and Special Case are not applicable when there is no valid object to return from a method

##Map-reduce pattern in domain logic
partition the data

apply the mapping function to each of the elements of the collection, obtaining another collection as the result

reduce function applied to mapped results

###Benefits
simplified code

readable code

short implementation

map-reduce enforces object-oriented design

##Iterator design pattern and sequences
`IEnumerator` is the .net implementation of iterator design pattern

`IEnumerable` - container role
`IEnumerator` - iterator role

`yield` produces sequences without having the underlying collection

Sequences decouple collection from iteration

if no collection is used a lot of memory is saved

code can become shorter and easier to undestand

###Infinite sequences
when there is no underlying collection sequence may run forever

c# allows processing of long sequences without using much memory

##Option functional type
special type for potentially missing objects

- _Option_ - Scala, OCaml, F#, Java
- _Maybe_ - Haskel, Idris

option either contains a value or it doesn't, but it is **never** null

C# can implement _option_ using a collection

```C#
public class Option<T>: IEnumerable<T>
{
    private readonly T[] _data;
    
    private Option(T[] data)
    {
        _data = data;
    }
    
    public static Option<T> Create(T element)
    {
        return new Option(new[]{ element });
    }
    
    public static Option<T> CreateEmpty()
    {
        return new Option(new T[0]);
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)_data).GetEnumerator();
    }
    
    IEnumerator IEnumerator.GetEnumerator()
    {
        return GetEnumerator();
    }
}
```

```C#
public bool IsRegistered(string userName)
{
    return
        this.userRepository()
            .Find(userName)
            .Any();
}

public void Deposit(string userName, decimal amount)
{
    return
        this.userRepository()
            .Find(userName)
            .ForEach(user=>user.Deposit(amount));
}

public IPurchaseReport Purchase(string userName, string itemName)
{
    return
        this.productRepository()
            .Find(itemName)
            .Select(product => Purchase(userName, product))
            .DefaultIfEmpty(this.reportFactory.CreateProductNotFound(userName, itemName))
            .Single();
}

```

`Option` type in F#
```F#
> let getPrice (itemName: string) =
    match itemName.Length with
    | length when length <= 6 -> Some((float)length * 3.14)
    | _ -> None;;
    
> let report (itemName: string) =
    match getPrice itemName with
    | Some(price) -> sprintf "You can have %s for $%f" itemName price
    | None -> sprintf "We don't sell %s" itemName

> report "laptop"
val it : string = "You can have laptop for $18.84"

> report "time machine"
val it : string = "We don't sell time machine"
```

##Service Locator vs Object oriented code
`DateTime.UtcNow` is a service locator

service locator is applicable only in procedural code (not object oriented)

1. construct dependency injector
2. resolve root object
3. invoke root object
4. terminate after execution

cannot create tests that rely on service locator (i.e. tests that rely on `DateTime`)

if a concept is a requirement do not use service locator for it
- use proper abstraction instead
- inject abstraction as dependency

to get rid of `DateTime`:
- use a `TimeServer` _abstraction_
- have a fake `TimeServer` _concrete dependency_ for testing
- have a real `TimeServer` _concrete dependency_ for production that uses `DateTime` _service locator_

##Applying Service Locator in the presentation layer
###when is Service Locator useful?

handle messages:
1. object oriented approach
    - message executes itself
2. procedural approach
    - message is passed to a procedure as an argument
    - procedure executes the message
    
use service locator to retrieve the service for message types

###Problems caused by the service locator
- hard to test
- hard to vary implementation

###Design issues regarding Service Locator

- hides the client's real dependencies
- client cannot use different service that the one of the service locator 

dependency injection is done properly only if

- dependencies are injected during initialization

using dependency injection later equals using service locator

legitimate uses of service locator
- mapping network messages
- mapping domain model to UI elements

service locator is useful:
- at the object oriented to non-object oriented code boundary

##Guard clause and If-Then-Throw pattern
- if-then can be used as a guard clause
- if-then-throw is meant to throw an exception under certain conditions
    * throw on null argument

guard clauses are if-then statemets without the else branch (which abandon the method)
- if-then-return
- if-then-throw

they implement (math) partial functions

Bertrand Meyer - Object Oriented Software Construction: Design by contract

Code contracts: have a separate implementation beside Contract definition? 

Code contracts: **Assert on Contract Failure** - to fail the automated tests on contract violation; it shouldn't be on in production

remove branching instructions:
    - modify the design to avoid branching

different kinds of branching instructions:
    - if-then-else on dynamic condition
        * eliminated by unifying control flows
    - if-then-else on static condition
        * eliminated by template method or strategy pattern
        
preconditions from Design by Contract
    - use code contracts library to assert preconditions
    
Null object and Special Case
    - helped remove null references
    - removed branching on null
Map reduce design pattern
    - helps removing loops
Iterator design pattern
    - points that sequence of objects is also an object
    - reinforced by IEnumerable interface and yield keyword
Option<T> functional type
    - remove remaining null references (and null reference checks)
Service Locator
    - useful to adapt to non-Object-Oriented areas
        * network
        * storage
        * user interface
If-then-throw
    - replace branching with code contracts
    

Design Patterns Library from Steve Smith
Introduction to F#, Oliver Storm
C3 Collections fundamentals, Simon Robinson
