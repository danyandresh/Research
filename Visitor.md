###Visitor

_Behavioural_ pattern

> Allow an algorithm to access the interior state of objects through methods defined on the object

####Components

* **Visitor** Contract for the algorithm to be performed on `elements`
* **Concrete Visitor** Concretion of an `visitor` algorithm to be performed on `element`s
* **Element** Contract defining the visit method to allow a visitor
* **Concrete Element** `element` concretion that decides what method to call on `visitor` provided
* **Object Structure** an iterator-like structure aimed to traverse the sequence of elements - calling visit method with a `concrete visitor`

####Similar patterns

* **Iterator** is iterating through a sequence of elements in an external manner (allowing the client to do what it wants with each element), while `visitor` lets the elements themselves to decide whether they accept a visitor (in an internal operation)
* **Strategy** `visitor` is more specialized than the `strategy` in that it is operating on a data structure
