###Prototype
> Factor new objects by copying an existing one

####Components

* **Prototype** Contract of the object, define cloning capability
* **Concrete Prototype** Concretion of `product`, implements cloning capability
* **Client** Creates a new object by asking the prototype to clone itself

####Works nicely with:

- `state` pattern
 
####Cons:

- deep vs. shallow copy problems