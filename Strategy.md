###Strategy

_Behavioural_ pattern

> By using a common contract for multiple algorithms, this pattern allows behaviour to be decided by an outside entity

####Components

* **Strategy** Common contract for multiple algorithms
* **Concrete Strategy** `strategy` concretion for an algorithm
* **Context** uses the algorithm through the `strategy`

####Similar patterns

* **Command**: is self-contained and doesn't provide any visibility on what it does; `strategy` is decided by and well known to the `context`
