#Inversion of Control
##Vocabulary
- Dependency Inversion Principle (DIP): **principle** used in architecting software
- Inversion of Control (IoC): specific **pattern** used to invert interfaces, flow and dependencies
- Dependency Injection (DI): **implementation** of IoC to invert dependencies
- Inversion of Control container: framework to do dependency injection
- interface: externally exposed way to itneract with something

###Dependency Inversion
- instead of lower level modules defining an interface that higher level modules depend on, higher level modules define an interface that lover level modules implement

> high-level modules should not depend on low-level modules; both of them should depend on abstractions

> abstractions should not depend upon details. details should depend upon abstractions

##Inversion of control
more of a high level pattern that can be applied in different ways to invert different kinds of control:

- control over the interface between two systems or components
- control over the flow of an application
- control over dependency creation and binding

whenever the creation of a dependency is moved away, is qualified as creatiopn inversion

##Castle Windsor


