###Mediator

_Behavioural_ pattern

> Reduces complexity by hiding it and loosing the coupling between objects that the mediator uses and objects using the mediator

####Components

* **Mediator** Contract
* **Concrete Mediator** `mediator` concretion that hides the `colleagues` interaction complexity
* **Colleagues** Participants in communication through the `mediator`

####Similar patterns

* **Facade**: is aimed for only one way communication and isolation of the subsystem; `mediator` should provide both-ways communication
* **Bridge**: both reduce coupling, yet `mediator` aims to provide simple contracts and is more focused on decoupling from subsystem dependencies
