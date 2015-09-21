###Bridge
> _Structural_ (architecture) pattern, uses a set of contract to decouple two subsystems so even radical changes in either won't impact the other

####Components

* **Abstraction** A subsystem portal
* **Implementor** Contract used by the `abstraction` to talk to a subsystem-specific implementation
* **Refined Abstraction** an `abstraction` version customized for a particular application
* **Concrete Implementor** subsystem specific concretion of an `implementor`

####Similar patterns

* **Adapter**: `bridges` separate subsystems, while adapters only make an object implement a _foreign_ contract
* **Facade**: `facade`'s intent is to hide subsystem complexity, not to isolate from that subsystem's details