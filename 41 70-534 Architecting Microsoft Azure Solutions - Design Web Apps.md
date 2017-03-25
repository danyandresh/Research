#Architecting Microsoft Azure Solutions (70-534): Design Web Apps
##Web Apps, IaaS VMs, Cloud Services

- Scaling Up: PaaS host or SQL host - give more processor, RAM, faster storage

- Scaling Out: change the number of PaaS app hostor SQL host machines (within a tier)

###Database Transaction Unit (DTU)

- measures the database activity

http://dtucalculator.azurewebsites.net

###Azure SQL Geo-Replication

replicate to another azure region

- standard: disaster recovery only, going out of service soon
- active: useful for high-volume read oriented workloads

###elastic database pool

- pool DTUs across a group of databases
- dbs can burst in utilization as necessary

can't have a poll across regions

eDTU

###Sharding

- splitting data across multiple databases
- split-merge tool used to move data from one shared to another
- dbs don't have to be in the same tier


###azure site extensions

- pluggable modules that can be added to web apps
- adding an extension restarts the app

http://siteextensions.net

###azure web deployment

- FTP
- Kudu: push code from git, etc.
- web deploy: build binaries before pushing them

###app service plans

- allows apps to share features and capacity
- allows aoolcation of excusive hosting VM capacity apps
- 5 tiers:
    * free, shared, basic, standard, premium
    * can change tier
    * can increase and decrease instance size
    
- apps in an app service plan must be in the same subscription and geographic location
- web app is associated with one app service plan
- apps can be moved to different app service plans as necessary
- clone app to move to a different region

###resource groups

container for resources

- all resources used for an application
- subset of resources used for an application
- allows grouping of resources for security and management
- resources should share same lifecycle (should be deployed at the same time)

- a resource can only belong to a single resource group
- resources can be in separate azure regions
- resources can be moved between resource groups
- can define a json template for deployment and configuration of the resources in a resource group

###azure web app high availability

- deploy app to multiple regions
- configure azure traffic manager to route traffic to instances of the app in different regions

####Azure Traffic Manager
works as an application and session-aware load balancer

controls the distribution of user traffic to endpoints including cloud services, websites, external sites and other Traffic Manager profiles

uses and intelligent policy applied to DNS queries (suffix: trafficmanager.net)

load balancing methods:

- failover, when primary failes go to the second
- performance, route traffic to the closest geographical endpoint
- round robin, distributes load across cloud services (each in turn)

###design data tier
- choose the appropriate storage option for an application for web data
- the appropriate option depends on the type of data

####options
- relational:
    * SQL server
    * Oracle
    * MySQL
    * SQL Compact
    * SQLite
    * Postgres
    
- key/value
    * storage
    * azure table storage
    * azure cache
    * redis
    * memcached
    * riak
- column family
    * cassandra
    * HBase
- Document
    * MongoDB
    * RavenDB
    * CouchDB
- Graph
    * Neo4J

####storage account

- blob storage -> blob containers -> blobs
- file service -> shares -> files and folders
- table storage -> tables -> entities
- queue storage -> queues -> messages

- up to 500TB data per storage account
- 20.000 IOPS (1kb messages)
- unlimited number of blob containers, tables, queues, and file shares per storage account
- soft limit of 20 storage account per Azure subscription
- limited to a region: entities in a storage container cannot be in multiple regions
- security boundary

####storage redundancy
- locally redundant storage: 3 copies within single facility
- zone redundant storage: 3 copies within multiple facilities in the region
- geographically redundant storage: 3 copies within single facility, 3 copies in second facility in separate region
- read-access geographically redundant storage: 3 copies within single facility, 3 copies in second facility

blob storage for : images, videos, audio, documents, log files, backups

blob stored in blob containers

blob throughput:

- 60 MB/seconds
- 500 transactions per second

tables:
- big data, key value, when partitioned load balances on multiple nods, optimistic concurrency

queues:
- low latency high throughput messaging system
- decouples components/roles
- asynchronous communication
- good for workflow apps

file service:
- shared folders in azure storage
- rest and SMB protocols

geo-redundant azure sql:

###deployments

deployment slot: 
- available on standard or premium tiers
- can only scale out on a production slot
- can't change tiers

###backups
- web app configuration
- web app file content
- azure SQL databases or MySQL databases connected to the app

- generates:
    * xml config
    * zip: db in BACPAC format
    
- `_backup.filter`  in the `wwwroot`

###restore web apps





