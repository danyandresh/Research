###Command

_Behavioural_ pattern

> Provides containment for a unit of work or a task

####Components

* **Command** Contract for a unit of work
* **Concrete Command** Concretion of unit of work
* **Invoker** `command` requester
* **Receiver** request handler, normally part of the `concrete command`

####Similar patterns

* **Strategy**: unlike `strategy` `command` is not known to the `invoker`; `strategy` is meant to perform a _specific_ task
