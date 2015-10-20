#AngularJS
##intro
angularjs.org
builtwith.angularjs.org
madewithangular
plnkr.co

###requirements
- add a `<script>` tag pointing to angular.js
```html
<script src="angular.js"/>
```
- add an `ng-app` attribute in your HTML
    * `ng-app` is an Angular directive
    * the `ng` is short for Angular

###revealing module pattern
encapsulate some code inside of a function, resembling a module

###immediately invoked function expression (IIFE)
```javascript
(function(){
    // do work
}());
```

##controllers
```html
<div ng-app>
    <div ng-controller="MainController">
    </div>
</div
```
```javascript
var MainController = function($scope){
    $scope.message = "Hello!";
};
```

###`ng-src`
prevents the browser attempting to load an image directly from a binding expression, allowing angular to replace binding expression first and then set it to `src`

###`$http`
- GET
- POST
- PUT
- DELETE
simply add the `$http` parameter to controller function to get access to it
always returns a promise
```javascript
var MainController = function($scope, $http){
    
    var promise = $http.get("/users/12345");
    
    promise.then(function(response){
        $scope.user = response.data;
    });
};
```

###modules
`angular` the single identifier that angular puts in the global namespace
`angular.module('<name of the module>', dependencies[])` - registers a module
controllers live in modules
```javascript
var app = angular.module('githubViewer', []);
```

```html
<body ng-app='githubViewer'>
</body>
```

###registering controllers to modules
using `app.controller('<controller name>', <controller instance>)`
```javascript
var controllery = function($scope, $http){
};

app.controller('controllery', controllery);
```

alternatively, to deal with minifiers, specify the name of controller parameters explicitly
```javascript
var controllery = function($scope, $http){
};

app.controller('controllery', ['$scope', '$http', controllery]);
```

##directives
directives are used to push data from the view back to the model

- binding directives: {{binding expression}}
- model directives: can move data back and forth between model and view; `ng-model`
- event directives: `ng-submit`, `ng-click`
- display directives: `ng-include`, `ng-show`, `ng-hide`

###`ng-model`
keeps data in sync between model and view
```html
<input type='search' placeholder='Search watermark' ng-model='username'/>
```
###`ng-click`
binds action (click) to a function
```html
<input type='button' value='Search' ng-click='search(username)'/>
```

###`ng-submit`
binds submit action to a function
```html
<form ng-submit='search(username)'>
    <input type='submit' value='Search'/>
</form>
```

###`ng-repeat`
```html
<htmltag ng-repeat="<element> in <list>"/>
```

###filters
provides model data formatting
_pipe_ data through a filter
```
expression | filtername:parameter
```
- `currency`
- `date`
- `filter`
- `json`
- `limitTo`
- `lowercase`, `uppercase`
- `number`
- `orderBy`
```html
<tagname>{{element.property_count | number}}</tagname>
```

###`ng-show` and `ng-hide`
work with _truthi_ properties

```
ng-show="user"
ng-hide="!user"
```

###`ng-include`
can import html from another source