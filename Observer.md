###Observer

_Behavioural_ pattern

> Notify interested parties when an object changes state

####Components

* **Subject** Publisher, responsible to notify `observer`s
* **Observer** Contract of subscribers to the notifications
* **Concrete Observer** `observer` concretion responsible for a unit of work when notified

####Similar patterns

* **Command**: `command`s are used for a wider range of units of work; `observer`s are used only for units of work on notification
* **Strategy**: `strategy` comprises operations that are called directly;  `observer` can deal with multiple subscribers at the same time, whose operations are called through notifications and not directly
