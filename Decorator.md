###Decorator

_Structural_ pattern

> Change behaviour or functionality of an object at runtime through _composition_

####Components

* **Component** Contract of the objects that can have functionality changed at runtime
* **Concrete Component** Object whose functionality is altered
* **Decorator** Component wrapper that conforms to the `component` and defines new functionality
* **Concrete Decorator** Implements the additional functionality of the `Decorator`

####Similar patterns

* **Adapter**: `adapter` focuses on the contract, `decorator` on functionality/behaviour
* **Chain of Responsibility**: focused on finding the right handler for messages
* **Composite**: Composite doesn't add functionality
