###Iterator

_Behavioural_ pattern

> Separates the iteration logic from the actual sequence representation

####Components

* **Iterator** Contract for elements access and traversal
* **Concrete Iterator** `iterator` concretion that keeps track of an iteration progress
* **Aggregate** Contract for creating an `iterator`
* **Concrete Aggregate** `aggregate` concretion is the data source and is responsible for creating `iterator`s

####Similar patterns

* **Visitor**: unlike `visitor`s, `iterators` are meant to not modify the iteration source (rather _project_ a new one for filtering/generating purposes)
