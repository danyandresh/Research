##MVC4
- `<meta charset=` - must be set to avoid XSS exploits https://www.owasp.org/index.php/XSS_Filter_Evasion_Cheat_Sheet
- `<meta name="viewport" content="width=device-width"` - important for mobile devices
- `modernizr` ensures html5 web app works fine on older browsers
- Views correspond in the `Views` folder to controller classes
- Controller pass result model to the view through `return View(model)`
- View is told what model is should use by `@model <Fully qualified class name>`
- Can add usings inside view using `@using <Namespace>`

###Unit testing
Testing controller - create an instance of controller and exert each of the actions

##Controllers
###Routing engine
responsible for routing a request (based on the URL) to the right action, determining controller, action and other request parameters
```c#
var controller = RouteData.Values["controller"];
var action = RouteData.Values["action"];
```
Actions typically return an ActionResult

- `ContentResult` - string literal; `Content`
- `EmptyResult` - no response
- `FileContentResult/FilePathResult/FileStreamResult` - returns the contents of a file; `File`
- `HttpUnauthorizedResult` - Returns a HTTP 403 status
- `JavaScriptResult` - returns script to execute; `Javascript`
- `JsonResult` - returns data in JSON format; `Json`
- `RedirectResult` - redirects the client to a new URL; `Redirect`
- `RedirectToRouteResult` - redirect to another action, or another controller's action; `RedirectToRoute/RedirectToAction`
- `ViewResult/PartialViewResult` - Response is the responsibility of a view engine; `View/PartialView`

###Action filters

- `OutputCache` - Cache the output of a controller
- `ValidateInput` - Turn off request validation and allow dangerous input
- `Authorize` - Restrict an action to authorized users or roles
- `ValidateAntiForgeryToken` - Helps prevent cross site request forgeries
- `HandleError` - Can specify a view to render in the event of unhandled exception

Custom `ActionFilter`s should derive from `ActionFilterAttribute`

##Razor Views
have `.cshtml` extension

obtain the virtual path of a view using `@VirtualPath`

razor views automatically html encodes text it presents (to prevent XSS)

use `@()` to delimit a `C#` expression or to mark an expression as `C#` (as opposed to raw text)

escape `@` (to be displayed literally) with another `@`

use `@:` to force transition from `C#` to text

`RenderBody()` - where view is rendered

`_ViewStart.cshtml` - defines what is the layout to be used by views; can define one in the views folder for a controller to get a custom one; also can change the `Layout` property on particular views

can define sections using `@section <section name>` in views, then the layout would place that section using `RenderSection()`

`Html` is a property on the `ViewPage` base class

`data-` attributes are used for client-side validation

`TryUpdateModel` - does the model binding 

`@Html.Partial()` - render a partial view

`@Html.Action()` - calls an action independently (as a sub-request) from the current action being served; must be coupled with that controller to return `PartialView`; action can be marked with `ChildActionOnly` attribute to disable direct access to it

##Entity Framework
SchemaFirst - database exists, EF models a created on that base

ModelFirst - models are created in a designer, then EF generates the `C#` classes and db schema

CodeFirst - `C#` classes are created first and EF can create the DB schema

EF can work with many relational databases: SQL Server, SQL Compact, Azure, Oracle, DB2

DB _context_ should inherit from `DbContext`; repositories are defined as properties - in this context - with type `DbSet<entity>` (equivalent of tables)

EF Connection string `(Localdb)\v11.0` is connecting to Local developer db, launched on demand (unlike sql express)

`DbContext` can call constructor with a specific connection string `: base("name=<connection string name>")`

EF migrations can help creating the db schema and seed the database

Use Package Manager Console to enable migrations: `Enable-Migrations -ContextTypeName <Name of db context>`

Update database `Update-Database -Verbose`

Run SQL code during the migration using `Sql("<sql code here>")`

Automapper is a tool for mapping from one object to another

Actions can mark bound parameters using `Bind` attribute on each param

Can mark properties that link to other tables with `virtual` to trigger data load from view; http://msdn.microsoft.com/en-US/data/jj574232

Overposting (mass assignment): sending too much data to a post request. http://odetocode.com/blogs/scott/archive/2012/03/11/complete-guide-to-mass-assignment-in-asp-net-
 
Overposting can be prevented using `[Bind(Exclude= "<property name>")]` or using a view model

Validation can be done with annotation attributes, custom validation attributes, or implementing `IValidatableObject`

##AJAX and ASP.NET
Bindling helps referencing file resources (scripts, css files) and can be controlled (to download bundled, minified versions) through web.config `debug=false`

`Ajax.BeginForm` performs an ajax operation, can update a portion of the screen by specifying `UpdateTargetId`

Controller can determine if current request is an ajax request by checking `Resuest.IsAjaxRequest()` method

`$(function(){ });` - DOM.OnReady IIFE

http://haacked.com/archive/2009/06/25/json-hijacking.aspx/

`web.config` from `Views` folder controls the razor views, adding an application wide namespace should happen there

##Security

###Windows Authentication

Uses `User.Identity` to determine currently logged in user

###Forms Auth

Always use `https` for login (`RequireHttps`)

`Authorize` can specify particular users, group names

`(SimpleRoleProvider)Roles.Provider`

`(SimpleMembershipProvider)membership.Provider`

Cross Site Request Forgery (CSRF): executing operations/actions on behald of an user without user knowing it (i.e. posting from a malicious form to a legitimate website on behalf of an user).
MVC fixes this through `AntiForgeryToken`s (that uensure the forms submitted originate from the legitimate application):

- Action should `ValidateAntiForgeryToken`
- View should have from generating `@Html.AntiForgeryToken()`

This works because a website cannot set cookies for another website + the form value has to match the value encrypted in a cookie

OpenId: http://openid.net/

OAuth: http://oauth.net/

DotNetOpenAuth: http://www.dotnetopenauth.net

##Infrastructure
###Caching
[OutputCache] cache the ouput of a particular controller action/child action in memory

- `VaryByParam`
- `Location`
- `VaryByHeader`
- `VaryByCustom`
- `SqlDependency`

recommended to use output cache profiles

###Localization
Thread.CurrentCulture property impacts formatting

Thread.CurrentUICulture impacts resource loading

Set the culture automatically based on `Accept-language` header using
```xml
<globalization culture="auto" uiCulture="auto"/>
```

* log4net http://logging.apache.org/log4net/index.html
* elmah http://code.google.com/p/elmah/
* P&P Application Logging Block

##Configuration

1. Machine machine.config
2. Machine web.config
3. Parent web.config
4. App web.config

Use `Add-Migration` to create a migration script in PMC

Deploying to Azure from VS can be done using a publishing profile





