#ASP.NET MVC 5 Fundamentals
##Upgrading from MVC4 to MVC5

- mvc package should be upgraded (as well as the other packages)
- .net framework (at least to 4.5)
- remove `{E3E379DF-F4C6-4180-9B81-6769533ABE47}` from `ProjectTypeGuids` (on project file)
##OWIN and Katana
- Katana is a web framework appearing with VS2013, built on specification named OWIN
- OWIN - Open Web Interface for .NET http://owin.org

Owin packages needed to create an Owin hosted app: `Microsoft.Owin.Hosting`, `Microsoft.Owin.Host.HttpListener`

###AppFunc

`Func<IDictionary<string, object>, Task>`

`Environment` = `IDictionary<string, object>`, request details according to OWIN specification

Components are chained together; each component should chain execution to the next one

Both Nancy and WebAPI (unlike ASP.NET MVC) do not depend on System.Web or ASP.NET

`Microsoft.AspNet.WebApi.OwinSelfHost` - package for WebApi

###WebApi component registration

Must define controllers and verbs, then register the routes (make the configuration)
```c#
private void ConfigureApi(IAppBuilder app)
{
    var config = new HttpConfiguration();
    config.Routes.MapHttpRoute("DefaultApi",
                                "api/{controller}/{id}",
                                new { id = RouteParameter.Optional });
                                
    app.UseWebApi(config);
}
```

###Hosting under IIS
- Use package `Microsoft.Owin.Host.SystemWeb`
- Turn the app to a class library
- Change the output to `bin\` (as opposed to `bin\Debug`)
- `"c:\Program Files\IIS Express\iisexpress.exe" /path:<path to the dll file>`

Configuration for Katana configuration discovery
```xml
<appSettings>
    <add key="owin:appStartup" value="KatanaIntro.Startup" />
</appSettings>
```
or
```
[assembly: OwinStartup(typeof(KatanaIntro.Startup))]
```
or use reflection to discover a `Startup` class to use for configuration

##Identity and security
`Individual accounts` - provides forms based authentication

Use `Authorize`, `AllowAnonymous` attributes to control security

Local db deletion should be notified to SQL Server, to avoid YSOD

Package `Microsoft.AspNet.Identity.Core` for user management

`UserManager` to be used for user management, not lower level components

If data persistence on RavenDB or MongoDB is needed, custom implementation of stores need to be provided to `UserManager`

Package `Microsoft.AspNet.Identity.EntityFramework` package containing implementations for the contracts defined by the above package like `UserStore` and `IdentityDbContext`

##Bootstrap

Front-end tool kit for web applications using HTML, CSS, JavaScript

Single Responsibility Principle applied to CSS design (breaks down various aspects of design into small composable classes)

Mobile first

`row` class - defines a 12 units row

`col-md-3` - a column that takes 3 units of the row's width

`col-md-offset-6` - a column starting at 6 units

`col-md-push-9` - pushes a column to the right with 9 from current unit

`col-md-pull-3` - pulls a column to the left with 3 from current unit

Bootstrap facilitates responsive design through CSS _media queries_

Four categories:
- Extra small   <768px
- Small         >=768px
- Medium        >=992px
- Large         >=1200px

_Polyfill_ (or a shim) - a library supposed to fill in wholes in browser unsupported features

`hidden-sm` - hidden on small devices; need to apply extra for _extra small_: `hidden-xs`

`visible-lg visible-md` do the opposite

`col-sm-6` - column take 6 units on small devices and above

http://getbootstrap.com/components/

`jumbotron`, `well` -emphasize a bit of text

`navbar-nav`, `nav-pills` highlight button

`nav-stacked`

`active`

`breadcrumb`

`pagination`

###Text components
`badge`, `label`, `label-danger`

`alert`, `alert-danger`

###Form components

`form-horizontal` - label and input side by side

`form-control` - input control

`control-label`

###Table components

`table`

`table-condensed`

`table-striped` - alternative background colour for rows

`table-hover`

`active`, `success`, `danger`

###Button components
`btn`, `btn-danger`, `btn-primary`

`btn-group`

`btn-lg`, `btn-xs`

###JS Components
`Modal`
```javascript
@section scripts{

    <script>
        $(function (){
            var showModal = function () {
                $("#theModal").modal("show");
            }
            
            $("#showModal").click(showModal);
        });
    </script>
    
}
```

```html
<div id="theModal" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                
            </button>
        </div>
        <div class="modal-body">
        </div>
        <div class="modal-footer">
            <button type="button" class="btn btn-default" data-dismiss="modal">
                Cancel
            </button>
            <button type="button" class="btn btn-primary">
                Save
            </button>
        </div>
    </div>
</div>
```

###Bootstrap themes
https://bootswatch.com/

##WebAPI 2
Package `Microsoft.AspNet.WebApi.HelpPage` - WebApi documentation package

Documentation:

- Xml documentation on actions
- project to build xml documentation file (to `App_Data`)
- `HelppageConfig.cs` -> `config.SetDocumentationProvider(HttpContext.Current.Server.MapPath("~/AppData/<xml file>"))`

###Attribute routing
WebApi support attribute based routing (i.e. Actions and controllers define the routing through attributes)

`[Route("patients/{id}/medications")]` - Get nested resources

`[Route("patients/{id:int:min(1)}")]` - Constraints

`[Route("patients/{id:int}")]
[Route("patients/{name:alpha}")]` - Multiple routes per action

`[Route("patients/{id?}")]` - Can apply to entire controller

`[RoutePrefix("api/patients")]` - Prefix for all other routes in controller

`[RouteArea("Admin")]` - Attributes for MVC Areas (https://msdn.microsoft.com/en-gb/library/windows/desktop/ee671793(v=vs.100).aspx)

```c#
WebApiConfig.cs

config.MapHttpAttributeRoutes();
```

###`IHttpActionResult`

Specialized responses like `Ok`, `NotFound`, etc. that will translate at execution time into Http responses with the right Http status codes

###CORS

Cross Origin Resource Sharing: allows Javascript in a browser to call an API on a different domain, (the target server must tell to the browser through headers the call is legal and it allows it, header is: `Access-Control-Allow-Origin: <allowed origin site>`; if header not present, browser does not allow it)

`Microsoft Asp.NET Web API Cross Origin Resource Sharing` package helps enabling CORS on a website

```c#
WebApiConfig.cs

var cors = new EnableCorsAttribute("*","*", "GET");
config.EnableCors(cors);
```

CORS can be enabled selectively on different controllers with `EnableCors` attribute

WebApi login is accessible from OAuth at "/Token" endpoint; request must include `grand_type=password` with the payload

Ajax requests should include header `"Authentication" : "Bearer " + access_token`

##EF 6
Supports multiple DbContexts in a project (pointing to the same database)

```
PM> enable-migrations -ContextTypeName IdentityDb -MigrationsDirectory DataContexts\IdentityMigrations
```

When `AutomaticMigrationsEnabled` is `false` migrations must be generated manually using `add-migration -ConfigurationTypeName <Configuration.cs file>`

`update-database -ConfigurationTypeName` 

###Scaffolding

Can generate controllers and views based on the model definitions

####Editor Templates
- have an `EditorTemplates` folder in the views (Shared, to have it available to all views or in each controller views folder to isolate it)

- add a partial view with name as the type of the property

- edit the partial view as necessary

Passing "" as name to `@Html.DropDown(<name to use>)` will fall back to using the name of the property (for which the editor is used)

###Async

For async to be effective an API to call in to must exist; EF supports Async through async methods: `ToListAsync()`, `FindAsync()`, `SaveChangesAsync()` etc.

Actions must be marked as async, return type is `Task<ActionResult>` and async methods called must be _awaited_

##Logging
DbContext has a log property to tap into communication with db server

Glimpse - logging tool for mvc apps

EF Log tap
```c#
Database.Log = sql => Debug.Write(sql);
```

Glipmse package for EF6; NAVIGATE TO `/glimpse.axd`

EF supports different schemas

On db contexts override `OnModelCreating`
```c#
protected override void OnModelCreating(DbModelBuilder modelBuilder)
{
    modelBuilder.HasDefaultSchema("<schema name>");
    
    base.OnModelCreating(modelBuilder);
}
```

##SignalR
Two way real time communication between browser and server

###Web sockets
use `ws` for simple connection and `wss` for encrypted connection 
```javascript
var socket = new WebSocket("ws://echo.websocket.org");

socket.send("<message>");

socket.onmessage = function(event) {
    alert(event.data);
}
```

SignalR package is `Microsoft.Asp.Net.SignalR`

Connections:

- WebSockets, with fall back to
- Server Sent Events (SSE)
- ForeverFrame
- Long Polling

On server side a hub is a class provided by SignalR to abstract the underlying connections

Connecting client to server

1. Define a `Hub` class on the server
2. Reference script `/SignalR/hub` to get the hub proxy available on the client
3. Setup reference to hub: `$.connection.<hub name>`
4. Setup logging (to JS console) `$.connection.hub.logging = true;` 
5. Start communicating: `$.connection.hub.start()`
6. Setup message receiver: `hub.client.<methodName> = function (<params>)`

##Web Dev Tools and VS 2013

###Browser Link
VS uses SignalR to control browsers where the application is loaded in

###Scaffolding

Add -> New scaffold item

Areas are techniques to break down large applications by functionality and isolate the pieces: controllers, along with models and views, area registration; areas are available through dedicated urls by default (`/newArea/<usual url>`)

Create a `CodeTemplates` folder on the root of project for the scaffolding to use custom templates.

Standard templates can be found under `c:\Program Files\Microsoft Visual Studio 12.0\Common7\IDE\Extensions\Microsoft\Web\Mvc\Scaffolding\Templates`

###Web Essentials

Web development extension for VS, allows Designing and editing mode of web apps (editing respectively sniffing content and where it is generated from in the application)

###Editor shorcuts

`Alt` + `1` - select hierarchical html tags towards the root of DOM
`Shift` + `Alt` + `W` - surround selected element with another

in CSS (highlighting a color) `Ctrl` + `Shift` + `Up`, `Ctrl` + `Shift` + `Down` make the colors lighter or darker

To work with LESS Web essentials extension is needed

###Zen coding

`div#content` + `Tab` -> to expand into a tag

`div>nav` +`Tab` -> nested elements

`li*5`

`li#item$$*5`

`div.container>(header>nav)+(div.row>div.col-md-4*3)`

###Side Waffle
manage project templates

###Azure tools
- import subscriptions










