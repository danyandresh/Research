###Event Aggregator

_Architecture_ pattern

> Simplify event registration using a centralized store and reduces coupling

####Components

* **Event Aggregator** Contract defining publish and subscription methods
* **Concrete Event Aggregator** concretion of `event aggregator` responsible for storing _weak_ reference to the subscribers and dispatching events to the right subscribers
* **Publisher** originator of events, consumer of `event aggregator`
* **Subscriber** contract defining the event subscription type 
* **Concrete Subscriber** concretion of `subscriber` responsible for executing a unit of work when particular event(s) are raised

####Similar patterns

* **[Facade](Facade.md)** an `event aggregator` is a _specialization_ ofr `facade`, focusing solely on _observer_ relationships
* **[Observer](Observer.md)** focuses more on a particular type of events, while `event aggregator` is being used in with a wide range of events as a central point of escalation for all notifications
* **[Strategy](Strategy.md)** is coupling client with `strategy concretion`, but focuses on a _single_ strategy at a time