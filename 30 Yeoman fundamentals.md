#y eoman fundamentals
##install yeoman
- install node
- install yeoman `npm install -g yo`
- install a generator `npm install -g generator-webapp`

run yeoman with `yo`

##sub-generators
`npm install -g generator-aspnet`

##create a generator
```
npm init
npm install yeoman-generator --save
npm link
yo <generator name>
```

Typescript intelisense for VS code
```
npm install tsd -g
tsd init
tsd install node lodash --save 
```
then the development env would pick up on intelisense:
```
var _ = require('lodash');

_. - have intelisense here
```

###private methods
prefix with `_`
use instance method
extend parent generator

###context methods
1. initializing
2. prompting
3. configuring
4. default
5. writing
6. conflicts
7. install
8. end

any custom method will be executed after `default`

##working with file system
###destination context
location where application is scaffolded
###template context
location where templates are stored

in memory file system to prevent accidental overwrite, synchronous file system API

convention:

> files with `_` at the begining of the name are the source templates

`this.templatePath()`

`this.destinationPath()`

`this.copy()` ~ `this.directory()`

`<%=  %>` - EJS - effective javascript templating

```
this.fs.copyTpl(
    this.templatePath('_index.html'),
    this.destinationPath('src/index.html'),
    {
        appname: 'a cool app',
        ngapp: 'myapp'
    })
```

`npm install` to install everything that is in the package.json file
`bower install` to install all bower dependencies


`gulp scripts` - verify scripts
`gulp watch` - creates a web server and watch the files for changes, reflecting the change immediately in the browser

##user interactions
`npm install yosay chalk lodash --save`
###arguments
```
module.exports = generators.Base.extend({
    constructor: function(){
        generators.Base.apply(this, arguments);
        
        this.argument('appname', { type: String, required: true});
        this.appname = _.kebabCase(this.appname);
        this.log('appname (arg):' +this.appname);
    }
});
```

###options
```
this.option('includeutils',{
    desc: 'Optionally includes Angular-UI utils library',
    type: boolean,
    default: false
});
```

```
if(this.options.includeutils){
    bowerJson.dependencies['angular-ui-utils'] = '~3.0.0';
}
```

###prompts
```
var done = this.async();
this.prompt({
    type: 'input',
    name: 'ngappname',
    message: 'Angular app name (ng-app)',
    default: 'app'
},{
    type: 'checkbox',
    name: 'jslibs',
    message: 'Which JS libraries would you like to include?',
    choices: [
        {
            name: 'lodash',
            value: 'lodash',
            checked: true
        },
        {
            name: 'Moment.js',
            value: 'momentjs',
            checked: true
        },
        {
            name: 'Angular-UI Utils',
            value: 'angularuiutils',
            checked: true
        },
    ]
}, function(answers){
    this.log(answers);
    this.includeLodash = _.includes(answers.jslibs, 'lodash');
    this.includeMoment = _.includes(answers.jslibs, 'momentjs');
    this.includeAngularUIUtils = _.includes(answers.jslibs, 'angularuiutils');    
    done(); 
}.bind(this));
```

##configuration and dependencies
```
{
    type: 'input',
    name: 'ngappname',
    message: 'Angular app name (ng-app)',
    default: 'app',
    store: true //will store this for later instance of the app
}
```

###storage api

```
this.config.set('ngappname', answers.ngappname);
this.config.save();

this.config.get('ngappname')
```

```
{
    type: 'input',
    name: 'ngappname',
    message: 'Angular app name (ng-app)',
    default: this.config.get('ngappname') || 'app'    
}
```

###install dependencies directly

```
//index.js generator file
//extended function
install: function(){
    this.bowerInstall();
    this.npmInstall();
    
    this.installDependencies({
        skipInstall: this.options['skip-install']
    });
},
end: function(){
    this.log(chalk.yellow.bold('Installation successful'));
    
    var hotToInstall =
        '\nAfter running ' + chalk.yellow.bold('npm install & bower install') +
        ', inject your fron end dependencies by running ' +
        chalk.yellow.bold('gulp wiredep') + '.';
        
    if (this.options['skip-install']){
        this.log(howToInstall);
        
        return;
    }
}

```

`gulp wiredep` are responsible for injecting the files into index.html

###generator composition
`composeWith(namespace, options, settings)`

`npm install -g generator-common`

`yo common --no-gitignore`

`npm install -g generator-common --save` - saves the common generator locally, in the current generator as a dependency 
```
git: function(){
    this.composeWith('common', {
        options: {
            'skip-messages': true,
            gitignore: true,
            gitattributes: true,
            jshintrs: false,
            editorconfig: false,
            'test-jshintrc': false
        }
    },
    {
        local: require.resolve('generator-common')
    });
}
```

##sub-generators
- create a sub folder with the name of the sub generator
- new file, `index.js`
- inherit from `generators.NamedBase` so the sub generator has a named argument by default

call subgenerator as 
`yo <generator name>:<subgenerator name> <name agrument>`

ng controller sample
```
(function (){
    'use strict';
    
    angular
        .module('<%= appName %>')
        .controller('<%= ctrlName %>', <%= ctrlName %>);
        
    <%= ctrlName %>.$inject = [];
    
    /* @ngInject */
    function <%= ctrlName %>() {
        /* jshint validthis: true */
        var vm = this;
        
        activate();
        
        function activate(){};
    }
});
```

controller generator:
```
// generators.NamedBase.extend({...
writing: function(){
    var fileNameFragment = getFileNameFragment(this.name);
    
    this.fs.copyTpl(
        this.templatePath('ng-controller.js'),
        this.destinationPath('src/app/' + fileNameFragment + '/' + fileNameFragment +'.controller.js'),
        {
            ctrlName: this.name,
            appName: this.config.get(ngappname)
        }
    );
    
    function getFileNameFragment(ctrlName) {
        var ctrlIndex = ctrlName.indexof('Ctrl');
        if(ctrlIndex === (ctrlName.length - 4)) {
            ctrlName = ctrlName.substring(0, ctrlIndex);
        }
        
        return _.kebabCase(ctrlName);
    }
}
```

##testing generators
`tsd install mocha --save`
`tsd install mocha --save-dev`
`npm test`

```
'use strict';
var path = require('path');
var assert = require('yeoman-generator').assert;
var helpers = require('yeoman-generator').test;

describe('yang:app', function(){
    describe('default', function(){
        before(function(done){
            helpers.run(path.join(__dirname, '../app'))
                .withArguments(['MyCoolApp'])
                .withOptions({ skipInstall: true})
                .on('end', done);
        });
        
        it('create files', function(){
            asset.file([
                'package.json',
                'src/app/app.js',
                '.bowerrc',
                '.gitignore',
                '.jshintrc',
                'bower.json',
                'gulp.config.js',
                'gulpfile.js',
            ]);
        });
        
        it('adds default ngapp', function(){
            assert.fileContent('src/app/app.js', /angular.module\('app/);
        });
    });
    
    describe('ngapp prompt', function(){
        before(function(done){
            helpers.run(path.join(__dirname, '../app'))
                .withArguments(['MyCoolApp'])
                .withOptions({ skipInstall: true})
                .withPrompts({ ngappname: 'fooBarApp' })
                .on('end', done);
        });
        
        it('injects custom ngappname', function(){            
            assert.fileContent('src/app/app.js', /angular.module\('fooBarApp/);
        });
    });
});
```