###Factory method
> Allow a subclass to decide which is the actual (concrete) type being instantiated

####Components

* **Creator** Defines the method needed to create the `product`
* **Concrete Creator** Defines method needed to create `concrete product`
* **Product** Product contract used by `creator`
* **Concrete Product** Actual type created by `concrete creator` and used by `creator` through the `product` interface

####Works nicely with:

- `strategy`
 
####Cons:

- enforcing implementation inheritance