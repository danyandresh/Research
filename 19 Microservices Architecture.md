#Microservices Architecture
###SOA

> A service-oriented architecture (SOA) is an architectural pattern in computer software design in which application components provide services to other components via a communications protocol, typically over a network. The principles of service-orientation are independent of any vendor, product or technology.

Caracteristics:

- Reuse functionality
- Service contracts and signatures do not change when service is upgraded
- Services are stateless

###Microservices Architecture
- Improved SOA
- SOA done well :)
- introduce a new set of additional design principles which teach one how to size the service correctly
- can scale properly and provide high performance
- a microservice provides a set of related functions to applications (do one thing and do it well)
- lightweight communication mechanism
- technology agnostic API
- each microservice has it's own data storage
- each microservice is independently changeable
- independently deployable
- a transaction can be completed by multiple microservices, therefore the transaction is a distributed transaction
- centralized tooling for the management of the microservices

- microservices are response to quick changes requests
- business domain-driven design
- release and deployment tools (on demand hosting)
- on-line cloud services
- asynchronous communication technology

- sorter development times,
- reliable and faster deployment
- enable frequent updates
- decouple changeable parts
- security: each microservice can have it's own security mechanism
- increased uptime
- fast issue resolution
- highly scalable and better performance
- better ownership and knowledge
- right technology
- enable distributed teams (as each microservice has it's own database)

##Microservices design principles
###High cohesion (Single responsibility principle)
The microservices content and functionality in terms of input and output must be coherent

Must have a single focus and the thing it does it should do well

- Single focus
- single responsibility
    * SOLID principle
    * Only change for one reason
- reason represents:
    * a business function
    * a business domain
- encapsulation principle from OOP
- easily rewritable code (as the service is very small)
- scalability
- flexibility
- reliability

###Microservices should be autonomous
A microservice should not change due to an external system that it interacts with had changed

- Loose coupling between microservices and their clients
- Honour contracts and interfaces (versions should not introduce breaking changes in contracts)
- stateless
- independently changeable
- independently deployable
- backwards compatible
- concurrent development

###Microservices should be business domain centric
- Service represents business function
- scope of service
- Bounded context from DDD
- identify boudaries/seams
- shuffle code if required in order to:
    * group related code into a service
    * aim for high cohesion
- responsive to business change

###Resilience of microservices
Embrace failure when it happens by degrading functionality or using default functionality

Microservices should register themselves when they start-up and if they fail should de-register, so the load balancer is aware only of the fully functional services.

- types of failure:
    * exceptions/errors
    * delays
    * unavailability
- network issues:
    * delay
    * unavailability
- validate input
    * service to service
    * client to service

###Observable microservices

Health check and monitoring (logs, activity, errors)

- centralized monitoring
- centralized logging

because of the distributed transactions, quick problem solving, quick deployment requires feedback, data used for capacity planning, data used for scaling, monitor business data, what is actually used

###Automate microservices
- tools to reduce testing
    * manual regression testing
    * time taken on testing integration
    * environment setup for testing
- quick feedback tools:
    * integration feedback on check-in
    * continuous integration
- quick deployment tools
    * pipeline to development
    * deployment ready status
    * automated deployment
    * reliable reployment
    * continuous deployment

- distributed system
- multiple instances of the service
- manual integration testing would be too time consuming
- manual deployment is time consuming and unreliable

##Microservices design
1. High cohesion
    - single thing done well
    - single focus
2. Autonomous
    - independently changeable
    - independently deployable
3. Business domain centric
    - represent business function or
    - represent business domain
4. Resilience
    - embrace failure
    - default or degrade functionality
5. Observable
    - system health
    - centralized logging and monitoring
6. Automation
    - tools for testing and feedback
    - tools for deployment

###High Cohesion
- identify a single focus
    * a business function, a function that has clear inputs and outputs
    * business domain, CRUD for a specific part of the organization
    
Example: Accounts should have separate microservices for CRUD and invoice generation, with separate databases respectively. There should be a separate microservice if an EDI invoicing is needed (to send invoices through Electronic Data Interchange).

- split into finer grained services
- avoid coupling multiple business functions into one microservice (avoid "Is kind of the same")
- prefer creating another microservice as opposed to overloading functionality of an existing one
- continuously question the design of microservices (code or peer reviews) if they indeed have a single reason to change

###Autonomous: Loose coupling
Autonomous = independently deployable and changeable

* loosly coupled: microservices should depend on each other in a minimal way
* communicate over the network
    - synchronous: straight away response to a request, then callback to the originator when the task is complete
    - asynchronous:
        * publish events as messages on a message queue
        * subscribe to events
* technology agnostic API: using Open Communication Protocol like REST over HTTP
* avoid client libraries
* contracts between microservices:
    - fixed and agreed interfaces
    - shared model
    - clear input and output
* avoid chatty exchanges between two microservices
* avoid sharing between services
    - databases
* should share data

###Autonomous: ownership
* microservice ownership by team
* responsibility to make autonomous
* agree contracts between teams
* responsibility for long-term maintenance
* collaborative development:
    - communicate contract requirements
    - communicate data requirements
* concurrent development
###Autonomous: versioning
* avoid breaking changes
* backward compatibility
* integration tests: test input, output and shared model
* versioning strategy
    - concurrent versions (old and new)
    - semantic versioning (Major.Minor.Patch)
        * Major increased when the new version **is not** backwards compatible
        * Minor increased when the new version **is** backwards compatible
        * Patch increased for defect fixes that are sill backwards compatible
    - coexisting endpoints (old code and new code)
    
###Business domain centric
- business function or business domain
- approach:
    * identify business domains in a coarse manner
    * review sub groups of business functions or areas
    * review benefits of splitting further
    * agree a common language
- microservices just for data (CRUD) or (business) functions
- fix incorrect boundaries
    * merge (microservices doing the same business function) or split
- explicit interfaces for outside world

###Resilience
- design for known failures
- failure of downstream systems
    * other services internal or external
- degrade functionality on failure detection
- default functionality on failure detection
- design system to fail fast
- use timeouts
    * for connected systems
    * timout requests after a threshold
    * service to service
    * service to other systems
    * standard timeout length
    * adjust lenght ona case by case basis
- network outages and latency
- monitor timeouts
- log timeouts

###Observable: Centralized monitoring
* real-time monitoring
* monitor the host (CPU, memory, disk usage, etc.)
* expose metrics within the services (response times, timeouts, exceptions/errors)
* business data related metrics (number of orders, average time from basket to checkout)
* collect and aggregate monitoring data:
    - monitoring tools that provide aggregation
    - monitoring tools that provide drilldown options
* monitoring tools that can help visualize the trends
* monitoring tools that can compare data across servers
* monitoring tools that can trigger alerts

###Observabe: Centralized logging
Monitoring is about collects stats and numbers, logging is about collecting detailed information about events

* When to log?
    - Startup or shutdown
    - Codepath milestones (requests, responses and decisions)
    - timeouts, exceptions and errors
* structured logging
    - Level: Information, Error, Debug, Statistic,
    - Timestamp
    - correlation id
    - host name
    - service name and service instance
    - message
* traceable distributed transactions
    - correlation ID: unique ID assigned to each transaction, passed service to service

###Automation: CI Tools
Continuous integration tools

- work with source control systems
- automatic after check-in
- unit tests and integration tests
- ensure quality of check-in
    * code compiles
    * tests pass
    * changes integrate
    * quick feedback
- urgency to fix quickly
- build creation
- build ready for test team
- build ready for deployment'

###Automation: CD tools
Continuous deployment tools
- automate software deployment
    * configure once
    * works with CI
    * deployable after check-in
    * reliable release at any time
- Benefits
    * quick to market
    * reliable deployment
    * better customer experience

##Technology for microservices
###Communication
####Synchronous
- Request/response communication
    * client to service
    * service to service
    * service to external
- RPC (Remote procedure call)
    * problematic, it is sensitive to change
- HTTP 
    * works across the internet
    * firewall friendly
- REST
    * CRUD using HTTP verbs
    * natural decoupling
    * open communication protocol (no technology imposed for client or server)
    * REST with HATEOS (technique to include links to related resources in responses)
- Synchronous issues:
    * both parties have to be available
    * given it is a distributed transaction slowdown of one service highly impact the overall transaction time
    * performance subject to network quality
    * clients must know the location (host/port) of the service; can be solved using discovery patterns
    
####Asynchronous
- Event based communication
    * mitigates the need of client and service availability
    * decouples client and service
- message queueing protocol
    * message broker (for receiving and storing messages, making them available to other clients)
    * subscriber and publisher are decoupled
    * microsoft message queueing (MSMQ)
    * RabbitMQ
    * ATOM(HTTP to propagate events)
- asynchronous challenge
    * complicated
    * reliance on message broker
    * visibility of the transaction
    * managing the message queue
- real world systems
    * would use both synchronous and asynchronous
####Hosting platforms: Virtualization
- uses a virtual machine as a host for a microservice
- foundation of cloud platforms
    * platform as a service, provides cloud services in order to run microservices architecture
        - azure
        - AWS
        - own cloud (e.g. vSphere)
- could be more efficient:
    * takes time to setup
    * takes time to load
    * takes resources
- unique features
    * take snapshot
    * clone VMs
- standardised and mature

####Hosting platforms: containers
- type of virtualization
- isolate services from each other
- single service per container
- different to a VM:
    * uses less resources
    * faster than VMs
    * quicker to create new instances
- future of hosted apps
- cloud platform support growing
- mainly linux based
- not as established as virtual machines
    * not standardised
    * limited features and tooling
    * incipient infrastructure support
    * complex to setup
- examples
    * Docker
    * Rocker
    * Glassware

####Hosting platforms: self hosting
- implement own cloud
    * own virtualization platform
    * own containers
- can use physical machines
    * single service on a server
    * multiple services on a server
- challenges
    * long term maintenance
    * need for technicians
    * training
    * need for space
    * scaling is not immediate

####Hosting platforms: registration and discovery
- where?
    * host, port and version
- service registry database
- register on startup
- deregister on failure
- cloud platforms make registration/deregistration automatic and simple
- local platform registration options
    * self registration
    * third-party registration
- local platform discovery options
    * client-side discovery (client connects to registration database to discover services)
    * server-side discovery (a load balancer connects to the database to discover the services on behalf of the client)

####Observable microservices: Monitoring tech
- centralized tools
    * Nagios
    * PRTG
    * Load balancers 
    * new relic
- desired features
    * metrics across servers
    * automatic or minimal configuration
    * client libraries to send metrics
    * test transactions support
    * alerting
- network monitoring
- standardise monitoring
    * central tool
    * preconfigured virtual machines or containers
- real-time monitoring

####Observable microservices: Logging tech
- portal for centralised logging data
    * Elastic log
    * Log stash
    * Splunk
    * Kibana
    * Graphite
- client logging libraries
    * serilog
- desired features
    * structured logging
    * logging across servers
    * automatic or minimal configuration
    * correlation/context id for transactions
- standardise logging
    * central tool
    * template for client library
    
####Microservices performance: Scaling
- how:
    * creating multiple instances of service
    * adding resource to existing service
- automated or on-demand
- PAAS auto scaling options
- virtualization and containers make scaling possible
- physical host servers is the slower option for scaling
- load balancers are needed
    * API Gateway
- scale up when:
    * performance issues
    * monitoring data
    * capacity planning
    
####Microservices performance: Caching
- caching to reduce 
    * client calls to services
    * service calls to database
    * service to service calls
- API Gateway/proxy level - the preferred caching level
- client-side (SPAs)
- service level (caching response from a service)
- considerations:
    * simpe to setup and manage
    * data leaks
    
####Microservices performance: API Gateway
API Gateways are the central entrypoint into the system
- helps with performance
    * load balancing
    * caching
- helps with
    * creating central entry point
    * exposing services to clients
    * one interface to many services
    * dynamic location of service
    * routing to specific instance of service
    * service registry database
- security
    * API Gateway
    * dedicated security service
    * central security vs service level
    
####Automation tools: Continuous integration
- TFS
- TeamCity
- Desire features:
    * cross platform: Windows builders, java builders, etc.
    * source control integration
    * notifications
    * IDE Inegration (desired)
- Map a microservice to a CI build
    * code change triggers build of specific feature
    * quick feedback just on that microservice
    * separate code repository for a service
    * end product in a single place
    * CI builds to test database changes
    * both microservice build and database upgrade are ready
- avoid a single CI build for all services

####Automation tools: Continuous deployment
- CD tools
    * cross platform tools
- desired features
    * central control panel
    * simple to add deployment targets
    * support for scripting
    * support for build statuses
    * integration with CI tools 
    * support for multiple environments
    * support for PAAS 
    
##Brownfield microservices architecture
###Approach
* Existing system could be
    - large monolithic system
    - organically grown
    - seems to large to split
* Lack of microservices design principles
* Identify seams
    - separation that reflects domains
    - identify bounded contexts
* start modularising the bounded contexts
    - move code incrementally
    - tidy up section per release
        * take time
        * existing functionality needs to remain intact
    - run unit tests and integration tests to validate changes
    - keep reviewing
* seams are future microservices boundaries
###Migration
* code is organised into bounded contexts
    - code related to a business domain or function is in one place
    - clear boundaries with clear interfaces between each 
* convert bounded contexts into microservices
    - start off with one
        * use to get comfy: CI, Tests, Deployment, development (separate database, etc.)
    - can make microservice and old functionality switchable (in case issues arise)
        * risk to maintain two versions of the same code
* how to prioritise what to split
    - by risk 
    - by technology
    - by dependencies
* incremental approach
* integrating with the monolitic
    - monitor both for the impact
    - monitor operations that talk to microservices
    - review and improve the infrastructure to support distributed transactions
###Database migration
* Avoid shared databases
* Split databases using seams
    - relate tables to code seams
* Supporting the existing application
    - data layer that connects to multiple databases
* Tables that link across seams
    - API calls that can fetch the data for a relationship
* Refactor the database into multiple databases
* Data referential integrity
* Static data tables 
    - can be solved by making a configuration file available to all microservices or a dedicated microservice
* Shared data
    - can be made available through a dedicated microservice
###Transactions
* Transactions ensure data integrity
* Transactions are simple in monolithic apps
* Transactions spanning microservices are complex
    - Complex to observe
    - Complex to fix
    - Complex to rollback
* Options for failed transactions
    - try again (message broker should place the transaction back on the queue for another microservice to pick it)
    - abort the entire transaction - problems around undo transaction failing itself, or determining who should initiate the undo transaction
    - use transaction manager software
        * two phase commit: transaction manager asks all participants for commit readyness, asks for commit and instructs for rollback if any of the participants fail to commit
    - disadvantage of transaction manager
        * reliance on transaction manager itself
        * delay in processing 
        * potential bottleneck
        * complex to implement
* Distributed transaction compatibility
    - notify the message broker of the monolithic service of the completeness of a transaction
###Reporting
* Microservices complicate reporting
    - data split across microservices
    - no central database
    - join data across databases
    - slower reporting
    - complicate report development
* Possible solutions
    - dedicated report service
    - data dump to a central location (OLAP)
    - consolidation environment

##Greenfield microservices architecture
###Introduction
* new project
* evolving requirements
* business domain
    - not fully understood
    - getting domain experts involved
    - system boundaries will evolve
* team experience with microservices (first project of this kind or experience devs?)
* existing system integration
    - monolithic system
    - established microservices architecture
* push for change
    - changes to apply microservices principles
* best to start off with a monolithic design
    - high level monolithic design
    - evolving seams
    - develop areas into modules
    - boundaries start to become clearer
    - refine and refactor the design
    - split further when required
* modules become services
* shareable code libraries promote to service
* review at each stage the microservices principles
* prioritise by:
    - minimum viable product
    - customer needs and demand
    
###Provisos
* accept initial expense
    - longer development times
    - cost and training for tools and new skills
* skilling up for distributed systems
    - handle distributed transaction only by experience
    - handle reporting
* additional testing resource
    - latency and performance testing
    - testing for resilience
* improving infrastructure
    - security
    - performance
    - reliance
* overhead to manage microservices
* cloud technologies
* culture change
    

