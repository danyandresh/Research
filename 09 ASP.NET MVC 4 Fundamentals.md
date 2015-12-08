#ASP.NET MVC 4 Fundamentals

`StructureMap` IoC can help with Controller dependencies

Fiddler can simulate _modem speed_

`window.maxConnectionsPerServer` - maximum number of connections per server that a browser allows

`Install-Package Microsoft.AspNet.Web.Optimization -Version 1.0.0-beta2 -Pre`

`WebGrease` - includes a tool (wg.exe) for minifying file, validating files (e.g. `.js`), building css sprites

Bundles are capable of working with wildchars, ignore vsdoc files, minified (in debug) or non-minified (if release)

Bundle _virtual path_ should reflect the actual path of the bundled files to avoid relative paths issues

Can use `ResolveBundleUrl` for fine grained control over cached bundled resources (to force refresh on change for instance - using the query string value that will change)

`BundleTable.EnableOptimizations` - can override the web.config _debug_ setting

###Bundling coffeescript files
`install-package CoffeeBundler`
```c#
var coffeeBundle = new ScriptBundle("~/bundles/coffee")
                        .Include("~/Scripts/test.coffee");

cofeeBundle.Transforms.Insert(0, new CoffeeBundler.CoffeeBundler());

bundles.Add(coffeeBundle);
```

##WebAPI
`ApiController` is the base class for all WebApi controllers

###Routing
Controller method names should start with the name of the verb (e.g. `GetBooks`, `PostBook`, `PutBook`)

WebApi Routes are registered using `.MapHttpRoute`, there is no action and path should start with `api`

`curl`(http://curl.haxx.se/download.html) can send web requests from the command line

IISExpress must be setup to support `PUT` and `DELETE` verbs (changing the `ExtensionlessUrl` setting)

Content negotiation depends on `Accept` header

`GlobalConfiguration.Configuration`

`GlobalConfiguration.Configuration.Formatters`

WebAPI basic type parameters have to be instructed to load from body with `[FromBody]` attribute; otherwise they are sought for in the querystring

WebAPI complex types are assumed to be in the message body

Url-Form encoded simple types are only passed as `=<value>` to the post payloads - as naming these simple values is not supported

`Handlebars.js` - a templating library for js

`@Url.RouteUrl("<Route name>", new { httproute = "", controller = "videos" })` - will construct a url based on that specific route; `httproute=""` property on the anonymous object is required

EF `db.Entry(model).State = EntityState.Modified` - marks the model as changed

`install-package Microsoft.AspNet.WebApi.Client` - install WebApi client in a regular app

`AsyncTimeout` attribute for async actions

Use `jquery.mobile` for mobile websites

MVC determines if the requesting browser is from a mobile and serve the views having `.mobile.cshtml` in their name and the views for desktop otherwise. (e.g. `Index.mobile.cshtml` for mobile - if any - vs `Index.cshtml` for desktop)

can use _media queries_ to determine the appropriate style (i.e. for mobile or for desktop)

MVC determines browser capability based on the list at `Microsoft.NET\Framework\v...\Config\Browsers`

###DisplayModeProvider

Registers display modes which will be queried at runtime for opportunity to override view

```c#
DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("Mobile"/*suffix for the view*/)
{ 
    ContextCondition = c => c.Request.Browser.IsMobileDevice
});

DisplayModeProvider.Instance.Modes.Insert(1, new DefaultDisplayMode("Silk"/*suffix for the view*/)
{ 
/// Can change the user agent from the browser to test this
/// c.Request.UserAgent - will always return the true user agent
    ContextCondition = c => c.GetOverriddenUserAgent().Contains("Silk")    
});
```

Can override the browser (when a specific version like mobile/desktop is sought) using:

```c#
HttpContext.SetOverridenBrowser(BrowserOverride.Mobile)

HttpContext.SetOverridenBrowser(BrowserOverride.Desktop)
```
A cookie named `ASPXBrowserOverride` is set for this to work.

`GetOverriddenUserAgent()` - extension method from `System.Web.WebPages` ns

 





