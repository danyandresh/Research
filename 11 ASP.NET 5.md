#ASP.NET 5
##Projects and Runtimes
###Three major changes

1. File based project system
2. Dynamic compilation (due to Roslyn compiler); new default folder structure (due to a new Core CLR runtime optimized for the cloud, in addition to the old CLR, The new CLR is available as a NuGet package - runs on Linux and OSX)
3. Unification: MVC6 == ASP.NET MVC + WebApi; no System.Web dependency; a single base controller class, a single set of attributes, one set of model binders

###wwwroot Folder
for static web application assets

###project.json
(Mostly .NET) dependencies file

A nuget package with `""` version implies to always use latest

###Different runtime versions

- aspnet50
- aspnetcore50

on each compilation the application is checked against both runtimes (as specified in the `frameworks` property of `project.json`)

###KRE (K Runtime environment)

`<current user>\.k\packages` contains packages

First location to look for dependencies is defined in the `global.json` file.

##ASP.NET MVC 6

Method for application configuration (Katana pipeline)
```c#
//Startup class
public void Configure(IApplicationBuilder app)
{
    ...
    app.UseMvc();
}
```

Method for services configuration (runs before application configuration above)
```c#
//Startup class
using Microsoft.Framework.DependencyInjection;

public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddMvc();
}
```

Routing model

```
app.UseMvc(routes => routes.MapRoute("default", "{controller=Home}/{action=Default}"));
```

Routing attributes

```c#
[Route("/[controller]"), Route("/")]
public class HomeController : Controller
{
    [Route("action"), Route("")]
    public IActionResult Default()
    {
        ...
    }
}
```

###Razor views

respect the same structure of folder as in previews versions of MVC

###DI
To solve dependencies for a controller constructor

Singleton: `services.AddInstance<IDependencyContract>(new DependencyConcretion())`; can use `AddSingleton(lambda expression)`
New instance for each Http request: `services.AddScoped`
New instance for each component during the same http request: `services.AddTransient`

###Configuration
add dependency `"Microsoft.Framework.ConfigurationModel.Json" : "1.0.0-beta2"`

```c#
// in startup constructor
IConfiguration config = new Configuration()
                            .AddJsonFile("config.json")
                            .AddEnvironmentVariables();
```
  
###WebApi
Same base controller

##ASP.NET 5 Tools
###DNVM
https://github.com/aspnet/Home/blob/dev/README.md

```
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "&{$Branch='dev';$wc=New-Object System.Net.WebClient;$wc.Proxy=[System.Net.WebRequest]::DefaultWebProxy;$wc.Proxy.Credentials=[System.Net.CredentialCache]::DefaultNetworkCredentials;Invoke-Expression ($wc.DownloadString('https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.ps1'))}"
```

```
dnvm list
```

`k` can launch commands defined in `project.json`

```json
"dependencies":{
    "Microsoft.AspNet.Server.WebListener":  "1.0.0.-beta3"
}
"commands": {
    "web": "Microsoft.AspNet.Hosting --server Microsoft.AspNet.Server.WebListener --server.urls http://localhost:1234" /*An assembly with a main entry point*/
    "kestrel": "Microsoft.AspNet.Hosting --server Kestrel --server.urls http://<domain name>:1234" /*An assembly with a main entry point*/
}
```

`k web`

###Installing frontend packages
`Bower` is a library utility (package manager) focusing on frontend development
`bower install jquery --save`
 
Bower installs packages in `bower_components` folder

`bower install` - re-download packages based on the (Bower) config file (bower.json)

```json
/*project.json*/

"scripts": {
    "postrestore": [ "npm install", "bower install"],
    "postbuild": "grunt build "
}
```
###Grunt for JS tasks
An alternative is gulp

add npm configuration file to project

```json
"dependencies": {
    "grunt": "^0.4.5",
    "grunt-babel": "^4.0.0",
    "grunt-contrib-concat": "^0.5.1",
    "grunt-contrib-uglify": "^0.8.0"
}
```

```
npm install
npm install grunt-cli -g

grunt
```

https://www.npmjs.com/

```js
grunt.LoadNpmTasks("grund-babel");
grunt.LoadNpmTasks("grund-contrib-concat");
grunt.LoadNpmTasks("grund-contrib-uglify");

var babelConfig = {
    files: {
        src: ["scripts/<js file to load>"]
        dest: "scripts/destination.js"
    }
};

var concatConfig = {
    files: {
        src: ["bower_component/jquery/dist/jquery.js", "scripts/destination.js"]
        dest: "wwwroot/scripts/destination.js"
    }
};

var concatConfig = {
    files: {
        src: "wwwroot/scripts/destination.js",
        dest: "wwwroot/scripts/destination.min.js"        
    }
};

grunt.initConfig({
    babel: babelConfig,
    concat: concatConfig,
    uglify: uglifyConfig
});

grunt.registerTask("build", ["babel", "concat", "uglify"]);
```

```
grunt babel
grunt concat
grunt uglify
```

###Deployment

`Kestrel` cross platform development web server

```
kpm bundle --no-source -o ...
```

VS2015 comes with git installed (as well as SCP- secure copy)

```
scp -r ... <cloud location>
```