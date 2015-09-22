###Proxy

_Structural_ pattern

> Interfaces the communication with a real object through a placeholder or a surrogate

####Components

* **Proxy** Concretion of `subject` that controls access to the `real subject`
* **Subject** Contract for both `proxy` and `real subject` (needed so they can be interchanged)
* **Real Subject** the object represented by the proxy

####Similar patterns

* **Decorator**: `decorator`'s intent is to to allow undecorated objects be accessed directly as well as allowing any necessary number of decorations (the latter wouldn't make much sense from a proxy perspective)
