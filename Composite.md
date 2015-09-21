###Composite
> _Structural_ pattern used to organize related objects with a common interface into tree-like hierarchies

####Components

* **Component** Contract of the objects in the hierarchy
* **Composite** A container for other `component`s in the hierarchy irrespective of their role (`composite`s or `leaf`s)
* **Leaf** A terminal components (that doesn't contain other components)

####Similar patterns

* **Chain of Responsibility**: Unlike `composite`, `chain or responsibility` focuses on running messages through a set of filters so each message is captured by the right filter(s); `composite` is more concerned with the organization and relations between objects 
* **Decorator**: is aimed to enrich/change the behaviour of a single (contained) object
