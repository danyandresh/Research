###Interpreter

_Behavioural_ pattern

> Helps interpreting a language leveraging _grammars_

####Components

* **Abstract Expression** Operation interpretation contract
* **Terminal Expression** Operation concretion for a terminal symbol
* **Nonterminal Expression** Operation concretion for a nonterminal symbol (a grammar rule)
* **Context** global information

####Similar patterns

* **Chain of Responsibility**: only in interpreter the components implement grammatical rules
* **Composite**: has the same implementation
