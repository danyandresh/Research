###Memento

_Behavioural_ pattern

> Encapsulates an object in complete opacity from the clients, so it can saved and restored reliably

####Components

* **Originator** Responsible for snapshot creation of its own current state
* **Memento** Container of an originator's snapshot
* **Caretaker** Stores mementos without operating on them

####Similar patterns

* **Command**: focuses more on units of work, while `memento` focuses on the state of an object
