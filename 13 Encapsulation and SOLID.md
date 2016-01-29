#Encapsulation and SOLID
##Reusable components
Encapsulation is about creating reusable components whose implementation details are not important in order to use them.

Code quality (or lack of) impacts long term _productivity_ (i.e. a matter of weeks)

Encapsulation is about having a guideline for writing code that is easy to understand by others not having the know-how one had when wrote the code

##Information hiding
From encapsulation perspective information hiding is more about implementation details hiding, to insulate consumer from the intricacies of achieving a goal.

##Command Query Separation (CQS)
Operations should be either commands or queries, but not both

Command: an operation that has observable side effect in the system

Query: an operation that returns data and does not mutate observable state; queries are **idempotent**

CQS is a _principle_ because it says there are some things that can be done with a language and should **not** be done

It is safe to call queries from commands, but not the other way around.

###[Postel's law (robustness principle)](https://en.wikipedia.org/wiki/Robustness_principle)

> Be very conservative in what you send, be liberal in what you accept

Corolary to this principle is the [**Fail-fast** principle](https://en.wikipedia.org/wiki/Fail-fast)

> immediately report at interface level any failure or condition that is likely to lead to failure

Fail fast with detailed exceptions; exceptions are not intended for end-users but for other programmers

The better a guarantee is given about the output of an object, the easier the object is to use

###Tester/Doer
states that if there is an operation that is not guaranteed to be legally performed it shoult come in pairs:

- a tester operation with a boolean indicating whether it is legal to perform the operation or not
- the doer of the real work

This pattern is not thread safe.

###Try<Operation>
returns boolean indicating the operation succeeded

This pattern breaks fluence

###Maybe
it is a list or collection with 0 or 1 elements (borrowed from functional programming); used to model the lack of respectively presence of a result.
 
Encapsulation is about making code easier to understand for other developers; this impacts the time needed to read the code hence allowing more time to write code.

Make invalid states as difficult as possible to create

The purpose of SOLID is to become more productive by making code more maintainable through decomposition and decoupling

Design smells that impose SOLID

- Rigidity: The design is difficult to change and fit with new requirements
- Fragility: the design is easy to break (by uninitiated devs)
- Immobility: the design is difficult to reuse 
- Viscosity: it is difficult to do the right thing
- Needless complexity (overdesign, gold-plating)

**S**ingle responsibility principle

**O**pen closed principle

**L**iskov substitution principle

**I**nterface segregation principle

**D**dependency inversion principle

##Single responsibility principle
A class should have only one reason to change

> do only one thing and do it well

##Open Closed Principle
###Reused Abstractions Principle
If there are abstractions (interfaces or abstract-based classes) and they are not being reused by being implemented by various different concrete classes, those abstractions are poor.

> Abstraction is the elimination of irrelevant and the amplification of the essential

Start with concrete behaviour and discover the abstractions as commonality emerges

###Rule of three
A general purpose solution should not be introduced until there are three cases that fit into the same generalization

If there are two examples that look similar it might not be sufficient to discover the axes of variability, in consequence it might not be sufficient to produce a good abstraction

> A class should be open for extensibility and closed for modification

With OCP only bug fixing is permitted in terms of changing a class.

Use _strategy_ to favour composition over inheritance and facilitate OCP.

##Liskov Substitution Principle
###Strangler pattern
Add new behaviour and implementations around the old system and gradually move clients from using the old system to the new one, thus obsoleting the old one.

> Subtypes must be substitutable for their base types

Alternative wording

> A client should be able to consume any implementation of an interface without changing the correctness of the system

_Correctness of the system_ = the superset of all the correct behaviour that a system might exhibit

Indicators of changing the correctness of the system

- code throws `NotSupportedException`; `ReadOnlyCollection<T>` from .NET is such an example. .NET breaks LSP
- lots of downcasts in code
- extracted interfaces; the more members an interface has the more likely it is to break LSP

Reused Abstractions Principle is a corolary to the LSP, as RAP is proving that the system can work with multiple implementation of an interface.

The more interaction there is between members of an interface the more likely there is to have violations of LSP 

##Interface segregation principle
Unit bias is a universal cognitive bias

ISP states that

> Clients should not be forced to depend on methods the do not use

An interface is not defined by a concrete class that implements it, but by a client that consumes it.

Favour role interfaces over header interfaces (extracted interfaces, name comes from the C++ headers)

In contrast with Header interfaces, role interfaces are interfaces with very few members. Extreme examples are interfaces with a single member, but they work well with LSP, as there are no interactions between members of the interface.

Interfaces with no members at all are _marker_ interfaces

##Dependency Inversion Principle
Following SRP one ends up with loads of classes; following ISP, one ends up with with loads of fine grained classes with a single method.

Objects are data with behaviour.

Functions are pure behaviour; no data captured inside.

Closures are capturing (closing over) external data; closures are behaviour with data

> High-level modules should not depend on Low-level modules. Both should depend on abstractions.

> Abstractions should not depend on details. Details should depend on abstractions.

Composite pattern works very well with the commands.

Decorator (russian doll model) is well suited to compose queries. 

Aspect Oriented programming refers to wrapping an inner call with new behaviour.

_Read through_ cache.

**Cross-cutting concerns** are aspects of a program that affect other concerns. These concerns often cannot be cleanly decomposed from the rest of the system in both the design and implementation, and can result in either scattering (code duplication), tangling (significant dependencies between systems), or both