#Angular Front to Back with WebAPI
Angular is currently the framework for building responsive client-side web applications

- expressive HTML
- powerful data binding
- clean programming
- built-in services

Asp.NET Web API

- http
- RESTful
- supports broad range of clients
- built on the .NET framework
- built-in features for routing, actions, and more
- easy to learn

Angular app can be comprised of multiple modules

`ng-app="<main module>"` - main module
`<ng-include="'<child module>'"` - inclusion of a child module
`ng-view` - modern way for sub models

```c#
// retrieves API services descriptions
ViewBag.DocumentationProvider = Configuration.Services.GetDocumentationProvider();
Configuration.Services.GetApiExplorer().ApiDescriptions;
```

###angular services for calling web service
####`$http`
built into angular core, direct communication with the server, convenience methods for http requests like GET, POST, PUT.

requests return promises

REST
- REpresentational State Transfer
- requests and responses involve the transfer of resources
- resources are identified with a URL> `/api/videos/`
- requests utilize a standard set of HTTP verbs: `GET | POST | PUT | DELETE`

####`$resource`

separate angular component: angular-resource

abstraction on top of $http for calling RESTful services

requires less code

the dependency for a resource is defined in a _min-safe array_

###CORS

cross origin resource sharing - allows many resources on a web page to be requested from another domain outside the domain for which the resource originated

####JSONP

browsers don't enforce the origin policy on HTML script tags 

JSONP uses a script tag, passing a special parameter to a JSONP-enabled server to provide call-back-based cross-domain requests.

###enable CORS in WebAPI service
- install the CORS package: `Microsoft.AspNet.WebApi.Cors`
- call EnableCors method during setup

`EnableCorsAttribute`

###serialization formatters

> serialization is the process of traslating data structures or object state into a format that can be stored or transmitted across a network connection

- `JsonIgnore`
- `DateTimeZoneHandling` - date time formatting, default standard ISO8601
- `Formatting` - json indentation
- `CamelCasePropertyNamesContractResolver`

```c#
config
    .Formatters
    .JsonFormatter
    .SerializerSettings
    .ContractResolver = new CamelCasePropertyNamesContractResolver();
```

##parameters
> a query string is the part of a URL containing data that does not fit conveniently into a hierarchical path structure

###route parameter
```javascript
function productResource($resource, appSettings){
    return $resource(appSettings.serverPath + 
                "/api/products/:search");
}

productResource.query({search: 'GDN'}, function (data){
    vm.products = data;
})
```

if the parameter is not defined in the URL will be automatically added as query string

use query strings for
- filters

use URL path for
- locators
- `/api/products/5`
- `/api/products/5/sizes`

##OData

versatile querying mechanism for data

- add the `EnableQuery` attribute
- actions should return `IQueryable`

`Microsoft.AspNet.WebApi.OData` package should be installed

odata query params are prefixed with `$`

property names are pascal cased

`/api/products?$filter=Price+gt+5`

- `$top` - top n results: `$top:3`
- `$skip` - skip n results: `$skip:1`
- `$orderby` - sort the results: `$orderby:"ProductName desc"`
- `$filter` - filters the results based on an expression: `$filter:"Price gt 5"`
- `$select` - selects the properties to include in the response: `$select:"ProductName, ProductCode"`

###$filter logical operators
`eq`, `ne`, `gt`, `ge`, `lt`, `le`, `and`, `or`, `not`

###$filter string functions
`startswith`, `endswith`, `contains`, `tolower`, `toupper`

http://www.odata.org


###security
- use `JsonIgnore` attribute as needed
- use the `EnableQuery` attribute sparingly
- limit the page size (throught `EnableQuery`); `EnableQuery(PageSize=50)`
- limit the available query options: `EnableQuery(AllowedQueryOptions=AllowedQueryOptions.Skil|AllowedQueryOptions.Top)`
- use validator functions; override `ValidateQuery`

##POST
posts data for a resource or set of resources without specifying an ID

used to
- create a new resource when the server assigns the Id
- update a set of resources

**not idempotent**

##PUT
puts data for a specific resource with an Id

used to
- create a new resource when the client assigns the Id
- update the resource with the Id

**idempotent**

```
productResource.get({ id: 0},
    function (data){
        vm.product = data;
        vm.originalProduct = angular.copy(data);
    })
```
 
```
Ok,
Create,
InternalServerError

```

##user authentication in WebAPI
###Authentication vs. Authorization
authentication:

* identity (who is the user?)
* registered user (local membership database, third party) 

authorization

* permission: what is the user allowed to do?, what resources can the user access?
* authenticated user

authentication request is URL encoded form value
```
userName=abc&password=abc&grant_type=password
```

enable cors for Oauth providers
```c#
context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin",
    new[]{ "http://localhost:52436"});
```

authorization

`Authorize`
`AllowAnonymous`

