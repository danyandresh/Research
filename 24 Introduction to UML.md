#Introduction
UML current standard is 2.4.1

Communication:
- precise
- standard
- common

Tools:
- formal
- informal

###Types of modeling
####Structural modeling
looking for nouns
####Behavioural modeling
looking for verbs

Diagrams:
   1. use case: sought functionality, features, work needing to happen
   2. sequence: lay out how things process, hot the different pieces interact
   3. state diagrams: looking at transitions of an object
   4. activity: flowcharts with enhanced functionality
   
####Building blocks
Boxes:
1. class: a _box_ with a class name, fields and method names
2. use case: an _elypse_
3. component: have the _component_ icon at the right top corner of the box: pluggable pieces of the software
4. node: a _cube_ often used in deployment diagrams to represent something physical, like a database server for instance

Interactions:
1. messages are ways to communicate between objects
    - line with filled arrow head from the caller to the callee, synchronous calls, response is expected
    - dashed line with open arrow head are responses for the sync messages
    - line with open arrow head are async calls
    
2. states: box with the state name and round corners
3. actions: box with rounded corners (too), context can indicate whether it is an action or a state

Relationships:
1. association: a line with numbers beside each end indicating the association type (factors of multiplicity)
2. generalization: a specialization of a class using an inheritance chain; line with enclosed hollow arrow head
3. implementation: dashed line with enclosed and hollow arrow head. implement an interface, the arrow points to the definition
4. dependency: dashed line with open hollow arrow head - a relationship where one object depends on another, arrow pointing to the dependency

Common extensions:
1. notes/annotations - rectangular blocks with a folded corner, allowing free text to be typed in
2. stereotypes: `<<Interface>>` - identifying the element on the diagram being an interface
3. iconic stereotype - a visual representation (an image) of a component

####Key considerations
1. keep diagrams clean
    - readable
    - focused
    - precise
2. goals
    - visualize
    - specify things: use the diagram components correctly
    - document 
3. keep the audience in mind

##structural diagrams (architectural diagrams)
define the blueprint (structure) of the project, identify the things that make up the system

- project vocabulary: project stakeholders understand each other
- structural relationships: between parts of the system, define the entire architecture

###class diagrams
identifies the vocabulary of the system

define relationships between entities in the system (nouns)

####relationships
- basic ("has a"): multiplicity - line with asterisks/numbers on both ends
- aggregation: whole-part - line with hollow diamond;
- composition: ownership - line with a filled diamond; the objects involved in composition (owned) do not often stand on their own
- uses: dashed line with hollow open arrows

###component diagram
help understanding the structure of the system as far as subsystems or components go

they identify the interfaces between different components in the system

interfaces (extension points):
    - realized (lollipop): interfaces implemented by the component - line with a circle at the end (and a name over it) and a square at the other end (a port)
    - required (half circle lollipop) line with half circle
    
components loosely connected: using interfaces that connect

components tightly connected: using ports with line between

connecting of components to the outside world: ports with hollow open arrows in and out

###package diagrams
logical container for other components, as the models grow in size

can contain:
    - classes
    - interfaces
    - components
    - nodes
    - diagrams
    - packages
    
use `::` for refering to subcomponents through nested packages, etc. e.g. `AccountServices::LoanProcessing::RateCalculator`

use dashed line with hollow open arrow head

###deployment diagrams
help understanding parts of the built/defined systems and how they map to the physical infrastructure of the deployment environment

nodes are represented by boxes or cubes (often with <<processor>> stereotype attached), 

stereotyping can contain _details_ like the minimum number of servers, what services are deployed on that server.

##behavioural diagrams
identify how things that make up the system interact with each other to give the features and functionality that the system requires

they are more about verbs and actions than nouns, entities or things

they model:
    - system logic
    - algorithms
    - flows

###use case diagrams
indetify user tasks,  system interactions

they focus on **what** not **how**

####actors
special kind of entity in a use case diagram

1. people - typically modeled as roles
2. generalizations on the actors, like individual customer and commercial customer
3. processes or other systems

####use cases
are modeled with a simple ellipse and the name of the use case within it

a use case is the name of the overall interaction, overall functionality or outcome from the system

can factor out common processes and tie them as dependencies (using a dashed line with a hollow open arrow head along with the `<<include>>` stereotype)

identify optional additions: special dependencies with `<<extend>>` stereotype; use case extends _base_ use cases, adding specialization to the already existing functionality

generalization of the use cases or inheritance; continuous line with hollow closed arrow heads. 

####scenarios
a use case can have many scenarios

- different states in the process
- branches
- extensions
- exceptions

#####use case vs. use case diagram
use cases are docs that spcify the pre-conditions, post conditions, steps that are taken, exceptions and extensions handling, in one word they outline the details

use case diagrams show how different actors use these use cases

solid line indicates the actor uses a particular use case

boxes on the use case diagram indicate the system in which a use case exist

###sequence diagrams
help identifying how objects in the system interact with each other to implement the defined functionality

focus on time/order, the order in which operations occur

####classes
are defined with a box, class name in it and a dashed vertical line underneath (lifeline);

the lifeline can tell how long an instance of it is around

can have a focus of control block (vertical filled segment on the lifeline) 
    - solid line with filled arrow head going out
    - dashed line with hollow open arrow head coming in

####messages
these lines can specify methods being called (messages), which can include _self messages_ too

destroy message is modeled using an `X` focus block (which is terminal for the lifeline of an object)

return message can specify return type or not, depending on the level of detail

asynchronous message makes a call and expects no return message

####structured control
* looping processing: an enclosing rectangle that captures the part of the caller and message and focusing block on the callee with an inverted tab that indicates the control structure used; guard message (under the inverted tab) that adds information to the control
    - tab name `loop`

* optional processing: same structure (inverted tab, etc.)
    - tab name `opt`

* conditional
    - tab name `alt` with dashed line for the _else_ segment, along with the guard conditions
    
* parallel, executes features in parallel
    - tab name `par`

###state diagrams
models states on a single object at a time, and how that object can transit from one state to another 

_reactive objects_ are objects that respond to external stimulis, things that happen outside of the object

####states
#####basic
rectangle with state name, rounded corners

#####internal behaviour
`entry / action()`

`do / action()`

`exit / action()`

#####special nodes
1. initial node: filled circle - the state of the object after it was created
2. final node: filled circle surrounded by another circle - lifetime of an object ends

####transitions
#####basic
from the initial node to a state

from a state to the final node

#####transition
solid line with an hollow open arrow head, from a state to another state

#####event
cause transitions to occur,  e.g. when scheduled time is reached or when processing complete with failure or success

####composite state
when two or more states can transition to a particular state it is easier to model that using a composite state (enclose those states into a single one that can transition entirely)

###activity diagrams
model workflows or general operations

####action
a single step modeled with a filled, rounded corners rectangle (similar to state) + title representing the action

####activity
consists of multiple steps that can be broken down in multiple actions or even its own activity diagram

activity is used to zoom to the right level of detail

as the action, it is represented as a filled rounded corners rectangle along with the title 

####special nodes
1. initiation: filled circle - where the activity flow starts
2. completion: circle with a solid circle inside it - where processing of this activity terminates

####flow control
1. decision/branch: a diamond with a single flow of operation coming into the diamond and multiple flows going out (each of the outgoing flows is named, and the flow whose statement is the truth for the operation currently being executed is followed)

2. forks and joins: model parallel behaviour; modeled with a rectange with a single flow coming in and multiple going out or multiple coming in and a single flow going out, respectively

####swim lanes
allow activity diagram to be broken into sections that are performed by different entities:
- types of people
- roles
- systems

##tools
Case type tools:
    - Visio
    - Visual Studio
    - Software Architect
    