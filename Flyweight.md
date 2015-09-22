###Flyweight

_Structural_ pattern

> Leverages _extrinsic_ state and _sharing_ to make objects smaller and minimize memory footprint.

####Components

* **Flyweight** Contract for messages that use _extrinsic_ state
* **Concrete Flyweight** `flyweight` concretion responsible for state computation (shared data) or extraction from extrinsic data
* **Unshared Concrete Flyweight** `flyweight` concretion responsible for state computation using internal state
* **Flyweight Factory** responsible for `flyweight`s instantiation (_singleton_ with more than one object instance)

####Similar patterns

* **Composite**: `composite` does not concentrate on minimizing the memory footprint of the components
