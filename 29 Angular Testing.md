#Angular testing
Hot Towel Yeoman Generator: http://jpapa.me/yohottowel

```javascript
describe.only('PeopleController', function() {
    var controller;
    var people = mockData.getMockPeople();
    
    beforeEach(function() {
        bard.appModule('app.people');
        bard.inject('$controller', '$log', '$q', '$rootScope', 'dataservice');
    });
    
    it('hello test', function() {
        expect('hello').to.equal('hello');
    })
});
```

`gulp test`

`gulp serve-specs`

Mocha and Chai Syntax http://chaijs.com