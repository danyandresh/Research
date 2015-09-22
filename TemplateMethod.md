###TemplateMethod

_Behavioural_ pattern

> Allow application specific operations to be carried by subclasses

####Components

* **Abstract Class** Implements the general algorithm
* **Concrete Class** Implement specific operations that are unknown to `abstract class` yet they are _expected_ by it

####Similar patterns

* **Factory Method**: focuses on object creation namely what (concrete) object should be created to the superclass doesn't need to decide this; `template method` forsees a set of operations will have to happen, yet superclass doesn't have to work with _specifics_ - in order to reuse code
