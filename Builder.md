###Builder

> Used to encapsulate and DRY construction of complex objects, decoupling different (visual, storage, transmission) representations of a `director` from its definition

####Components

* **Director** The director of the building process (this is the _source_ object)
* **Builder** Contract for the construction that will employ `director`
* **Concrete Builder** Concretion of `Builder` that builds the `product`
* **Product** Product contract for the result

####Similar Patterns

* **Bridge**: unlike `bridge`, `builder` relies on a high coupling to do its work and it is not aimed to isolate two systems; it is rather aimed to keep implementation details (of a director into a different structure) contained. `Builder` is the _transformation_ pattern.
* **Visitor**: is focused more on dealing with `director`s (i.e. pull/change some data on it), `builder` is rather extracting that data organizing it in a new structure needed for interaction with a UI or a transmission operation (e.g. build a form, a SOAP message, etc.)
 
