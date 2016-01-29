#Tactical Design Patterns in .NET: Managing Responsibilities
##The right time to apply a design pattern
- Observer design pattern is supported by the C# language
    * Keywords `delegate` and `event` had to be added to the language to support it

- Implementation is not the same as principal design from the literature

Other design patterns part of the framework:

- Factory Method - `TryParse`
- Builder - connection string builder classes

Design patterns can rarely be implemented as out-of-the box solutions, attempts to make pattern libraries have failed

Code generation improves chances of success

    * EF is applied through generated code
    * Runtime Callable Wrapper is generated from the COM interface
    
- `IEnumerable` was used as base for LINQ library, extension methods, dynamic types, Func and Action

- `IComparable` and `IComparer` 

- `IEquatable`
    * Important when implementing Value Object design pattern
    * Methods not mentioned in the interface are required: GetHashCode, ==, !=
    * C# unable to communicate the whole pattern through the interface

When should be design pattern be applied? When the design is ready to accept it

> Man not only reacts passively to incoming information, but creates intentions,
> forms plans and programs of his actions, inspects their performance and
> regulates his behaviour so that it conforms to these plans and programs;
> finally, he verifies his conscious activity, comparing the effects of his
> actions with the original intentions and correcting any mistakes he has made
> - Alexander Luria, The Working Brain, Basic Books, New York 1973

Intentions - Plan - Regulation - Verification

Intentions: Requirements gathering

Plan: Analysis

Regulation: Implementation

Verification: Acceptance testing

Multiple interations:

- The first attempt is to produce the tailored design
    * Design patterns do not fit into this phase
    
- Then refactor to reach a better design
    * This is where design patterns fit well
    
##Cascading factories to eliminate dependencies
###Abstract Factory
> Provide an interface for creating families of related or dependent objects
> without specifying their concrete classes

Concrete factory keeps the dependency, not _abstract factory_

Concrete products leak into the concrete factories

##Composite design pattern

Letting the classes do many thing and the restricting the behaviour in code is not the best tactic;
it is much better if the actual roles can be recognized in the system and the represent them explicitly in the class model

###Abstraction vs Implementation

Fine grained responsibility classes work better, although the class diagram could be convoluted at first sight

##Compositiong the control role
The composite design pattern removes the responsibility of organizing components from the client to its own dedicated component

Construction of the composite elements should be controlled in order to avoid surprises

Trigger for composite design pattern: growing client iterating through a collection of child elements

###CQS and Composite
Commands blend easier with Composite (unlike Queries)

##Object composition using Chain of Responsibility
###Chain of responsibility
> Avoid coupling the sender of a request to its receiver by giving more than one object
> a chance to handle the request. Chain the receiving objects and pass the request along
> the chain until an object handles it

Transforming the chain of responsibility into an interface that can execute certain operations is called _dynamic downcast_

List of `object`s in a strongly typed language is a code smell, indicating a problem with the design

The problem of accessing the contained functionalities (on inner components) can be
 solved by pushing the dynamic casting to the components themselves; linking components
 provides the chains the responsibility
 
##Visitor Design pattern 
###Behavioral design patterns

* Widely used
    - Chain of responsibility
    - State
    - Strategy
    - Template Method
    
* Built into framework
    - Command
    - Iterator
    - Observer
    
* Narrowly scoped
    - Interpreter
    - Mediator
    - Memento
    
###Visitor design pattern
> Represents an operation to be performed on elements of an object structure.
> Visitor lets you define a new operation without changing the classes of the
> elements on which it operates

Visitor is not suited in systems where lots of new classes are added

> Keep data encapsulated, expose behaviour

Concrete visitor objects are tipically stateful

Concrete visitor typically has to produce one integral result of its overall work

> Do not let data leak out fo the domain model objects

Domain model yeilds data through visitor specialized methods i.e.
```c#
visitor.VisitCar(modelname, engineCapacity, fabricationYear)
```

It is a bad design to have models expose data and behaviours;
objects designed for data storage with no domain role (and no intrinsic behaviour) can exponse that data through accessors, domain models can't (as they have behaviour)

##Calling protocols and the visitor

Can define dedicated DTOs (that will have no behaviour attached) to ease communication between domain model and visitor

Two kinds of visitors:
* Command visitors
* Query visitors

`Save` visitor can enqueue all the details of a model, call internal save (that is establishing the order of database operations) and each save operation decides whether it has enough info to execute or whether it was already executed.

The visitor must adapt to any possible implementation and must work correctly in all cases

Visitor factories should be provided to accept methods as opposed to visitors instances

##Mixin and responsibilities

> A mixin class is a class that's intended to provide an optional interface or functionality
> to other classes. It's similar to an abstract class in that it's not intended to be
> instantiated. Mixin classes require multiple inheritance

What is a pattern?

> Each pattern describes a problem which occurs over and over again in our environment,
> and then describes the core of the solution to that problem, in such a way that you
> can use this solution a million times over, without ever doing it the same way twice
> - Alexander et al. A Pattern Language

As C# does not support multiple inheritance, Mixins can be implemented using interfaces.

Mixins let another class augment the original class with new feature

Design patterns emerge out of the design, not the other way around

Further learning:

- Design patterns library, Steve Smith
- SOLID Principles of Object oriented Design,  Steve Smith
- Encapsulation and SOLID, Mark Seeman
- Domain-Driven design fundamentals,  Steve Smith, Julie Lerman
- C# Programming paradigms, Scott Allen

- Design Patterns: Elements of Reusable Object-Oriented Software, Gamma et al.
- Object Design: Roles, Responsibilities and Collaborations, Rebecca Wirfs-Brock and Alan McKean
- Agile Principles, Patterns and Practices in C#, Robert C. Martin
- Refactoring: Improving the Design of Existing Code, Martin Fowler
- Domain-Driven Design: Tackling Complexity in the Heart of Software, Eric Evans
- Implementing Domain-Driven Design, Vaughn Vernan
- Real-World Functional Programming: With Examples in F# and C#, Tomas Petricek



