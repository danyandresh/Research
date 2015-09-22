###State

_Behavioural_ pattern

> Change behaviour depending on state through state-dependent implementations

####Components

* **Context** Encapsulates potential states, current state and defines methods for the outside world
* **State** Contract defining all methods that change behaviour on different states
* **Concrete State** `observer` concretion responsible for a unit of work when notified

####Similar patterns

* **Strategy**: is implemented by a state, yet it is not provided by an outside entity
