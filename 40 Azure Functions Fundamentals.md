#Azure Functions Fundamentals

> Process events with serverless code

Azure Functions: event + code

- azure VM (IaaS)

- azure cloud services:
    * web roles
    * worker roles
    * IIS and .NET pre-installed
    * automatic scaling

- azure web applications & web jobs:
    * easy to deploy
    * frameworks choice
    * hosted in a hosting plan
        - combine many sites on one server
        - scale up to many servers
    * web jobs
        - simplified backgroun tasks
        - the basis for azure jobs
      
##azure functions

- simplified programming model
    * just the code to respond to the event
    * no boilerplate
    * focus on the business req
    
- new pricing model
    * pay as you go
    * only pay as you go
    
    
##benefits

- rapid and simple development model
    * code within portal
    * eliminate boilerplate
    
- all the power of azure web apps
    * CI, Kudu, Easy Auth, Certs, Custom Domains
    
##serverless:

- delegated server management
- third party PaaS
    * DocumentDb, Auth0

- run custom code on azure functions
    * respond to events
    * functions as a service (FaaS)
    
- microservices architecture (nanoservices)

##function apps

group functions to allow sharing resources and configuration

##benefits

- good for quick experiments and prototyping
- automating development process
- decomposing or extending monolithic apps
- independent scaling
- adapters for integrating systems

##triggers and bindings

docs.microsoft.com/en-gb/azure/azure-functions/functions-triggers-bindings

```
//writes the output item to the queue
// uses IAsyncCollector because of the async mechanism being used
public static async Task<object> Run(HttpRequestMessage req, TraceWriter log, IAsyncCollector<Payload> outputQueueItem)
```

- webhook mode
    * built-in support for GitHub and Slack
    * Generic webhook expects JSON
- authorization keys
    * can choose anonymous mode
- restrict http methods

configuration ends up in function.json file

load shared file 
```
#load "../Shared/OrderHelper.csx"
```

`IBinder` to handle output manually

##deployment

npm package

```
 npm i -g azure-functions-cli
```

- manual deployment:
    * Kudu, FTP, WebDeploy
    
- continuous deployment
    * git repository
    
CHRON expressions

##security

standard mode
`?code=`
`x-functions-key`

webhook mode

`?code=...&clientid=`
```
x-functions-clientid
x-functions-key
```

http://docs.microsoft.com/en-us/azure/azure-functions

