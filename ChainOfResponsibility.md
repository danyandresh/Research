###ChainOfResponsibility or ChainOfCommand

_Behavioural_ pattern

> Runs messages through a set of handlers, giving the right handler the chance to handle them.

####Components

* **Handler** event-handling contract
* **Concrete Handler** Handles some messages
* **Dispatcher** Routes the messages through the set of handlers

####Similar patterns

* **Composite**: is very similar to `chain of responsibility`, the ordering and relationship is important in both, with the difference that the latter is concerned with the functionality of a specific configuration (i.e. how messages are handled and how that is impacted by the change in the configuration)
