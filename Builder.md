###Builder
> Used to encapsulate and DRY construction of complex objects

####Components

* **Director** The director of the building process (this is the _source_ object)
* **Builder** Contract for the construction that will employ `Director`
* **Concrete Builder** Concretion of `Builder`
* **Product** Product contract

####Works nicely with:

- OO design, to decouple different (visual, storage, transmission) representations of a `directory` from its definition
 
