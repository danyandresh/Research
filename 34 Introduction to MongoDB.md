#Introduction to Mondo DB
_impedance mismatch_: discrepancy between object in memory representation and itțs storage structure

no impedance mismatch with mongo

##scalability: locking
in relational systems:

- transaction spans rows and tables
- rigid consistency model
- scaling out, sharding worsen the problems

###solutions
- table denormalization
- relaxed consistency

MongoDB:

- no schema
- single document write scope: shorter scope
- eventual consistency: custom commit level (e.g. can wait for replicas to write doc, or can wait only for primary server, or can not even wait for the primary and go ahead)
- capped collections: fixed size collections that override older documents

consistency choices:
- complete
- majority
- fire and forget

##installation

`mongod.cfg`
```

systemLog:
    destination: file
    path: C:\mongodb\log\mongod.log
storage:
    dbPath: C:\mongodb\data
```

```
mongod.exe --config "C:\Program Files\MongoDB\Server\3.2\bin\mongod.cfg" --install
```

##replica sets
- primary (write location)
- secondary (read relicas, can't write)
- arbiter: only around for an election in case the primary fails.

`"priority" : 10` - defines the primary node

`"arbiterOnly" : true` - arbiter

##the mongo shell
* single command

```
mongo server1/admin --eval "db.runCommand({logRotate:1})"
```

* script command

```
mongo server1 myDailyChores.js
```

* script interactive

```
mongo server1 myDailyChores.js --shell
```

```
printjson(db.runCommand({logRotate:1}))
```

```
var userCount = function(){
    var count - db.Users.count();
    var entry = {_id:Date(0, n: count};
    
    db.UserCountHistory.save(entry);
    
    print("\nToday's User Count: " + entry.n);
};

userCount();
```

prevent dropping the database

```
DB.prototype.dropDatabase = function(){
    print("Can't drop it");
}

db.dropDatabase = DB.prototype.dropDatabase
```

CMD shortcuts:

* `Ctrl + K` drops the rest of the line
* `Ctrl + L` clear the screen
* `set EDITOR="notepad++.exe"`; calling `edit aJavascriptFunction` edits it
* `pwd()`
* `load('safer.js')`

###user RC file

`c:\users\<user>\.mongorc.js` - can store javascript to be executed each time the shel is opened; can contain the above mentioned example for preventing database drop

`mongo --norc`

```
var _no_ = function(){ print("Nope!"); }

DB.prototype.dropDatabase = _no_;
db.dropDatabase = db.prototype.dropDatabase;

DB.prototype.shutdownServer = _no_;
db.shutdownServer = db.prototype.shutdownServer;
```

##storage

##data storage format
uses the very efficient data storage format [BSON](http://bsonspec.org/)

##data storing rules

1. document must have an _id field
2. very few rules (in fact a single one); current document size 16 MB; if more than that is needed it must be split across multiple documents

##collections
a collection in mongo defines the scope of interaction with documents.

collection is automatically saved inside the document as `"ns"` property

##document id

`_id` cannot be an array (can get around this rule by converting the array to BIN(ary) data structure)

##objectId
- automatically assigned to documents without ids specified explicitly

can be generated on demand using `ObjectId()`

contains a timestamp: `ObjectId().getTimestamp()`

using `ObjectId` makes insertion roughly in ascending order and pretty fast

it would be faster to read docs when having custom document ids

##writing ops
`save` with the same id results in document overwriting

`insert` with the same id fails; always do inserts specifying ids; even better when specifying meaningful ids

`pretty()` - to pretty print json documents

`findOne({_id:1})`

###stale issues

solved with `update()`: atomic within a document:
```
db.<collection>.update(<query>, <update>, <options>)
```
options:

- one
- many
- upsert (insert if none matches)

```
db.a.save({_id:1, x:10})
db.a.update({_id:1}, {$inc:{x:1}})
db.a.update({_id:1}, {$set:{y:3}})
db.a.update({_id:1}, {$rename:{'Naem': 'Name'}})
db.a.update({_id:1}, {$push:{things: 'one'}})
db.a.update({_id:1}, {$addToSet:{things: 'one'}})
db.a.update({_id:1}, {$pull:{things: 'one'}})
db.a.update({_id:1}, {$pop:{things: 1}}) /*remove the last element in the array*/
db.a.update({_id:1}, {$pop:{things: -1}}) /*remove the first element in the array*/
```

applying array operator from above to a non-array type would fail

```
db.a.update({_id:1}, {$addToSet:{ things: 'one'}}, { upsert: true })
db.a.update({_id:1}, {$addToSet:{ things: 'one'}}, { multi: true })
db.a.update({ things: 'two'}, {$addToSet:{ things: 'one'}}, { multi: true })
```

###`findAndModify`

```
db.foo.findAndModify({
    query: <document>, /* which document */
    update: <document>, /* what change */
    upsert: <boolean>, /* create if none matching*/
    remove: <boolean>, /* can remove that document*/
    new: <boolean>, /* findAndModify returns the document before the change normally,
                       will return the updated version if this flag is true*/
    sort: <document>, /* query order */
    fields: <document> /* can filter out the fields needed to be returned  */
});
```

###`find()`

```
db.foo.find(query, projection)
```

`$in`

`$all` - used for arrays containing _all_ values

should use the _dot-notation_ to drill into sub documents

check inexistence of a field with `$exists`
```
db.animals.find({"info.canFly": {$exists: false}})
```

`,` behaves like _and_
```
db.animals.find({"info.type": 'bird', tags: 'ocean'})
```

###projection

include or exclude fields from projection by specifying `1` respectively `-1`

##cursors
```
var c = db.animals.find({}, {name:1})
c.size()
c.hasNext()
c.forEach(function(d){print(d.name)})
```

###sorting with cursor
```
var c = db.animals.find({}, {name:1}).sort({name:1})
```

###`limit()`ing

```
var c = db.animals.find({}, {name:1}).sort({name:1}).limit(3)
```

###`skip()`ing

```
var c = db.animals.find({}, {name:1}).sort({name:1}).skip(2).limit(2)
```

`limit`ing and `skip`ing provides efficient paging directly on the database side

##`findOne()`

works like find, except it returns a single object

##indexing
scan is bad

an index holds mapping to document locations from field values 

sort uses index

###index types
* regulat (B-Tree)
* geo (proximity of things)
* text (parsing text queries and comparing them against text fields)
* hashed (mainly for sharding, index on a certain field but have values evenly distributed)
* ttl (time to live, expiring documents - automatically removes documents)

```
db.foo.ensureIndex(
    keys, /* which fields, in what order, geo/text */
    options /* name, build now, unique, sparse, TTL, language */)
    
db.system.indexes.find({ns:'test.animals'},{key:1})
```

###`explain()` indexes

```
db.animals.find({name:'cat'}).explain()
```

`"cursor": "BasicCursor"` - no index

`"cursor": "BtreeCursor name_1"` - a Btree index

`"nsscanned": 1` - the number of scanned objects

```
db.animals.dropIndex(<index name>)
```

indexes with unique values would prevent writing writing duplicate documents

indexes store a null for documents that do not have that field

a _sparse index_ would only consider document that have that field

indexes built in the background may take much longer than the ones in the foreground

index names should not exceed 128 characters long

!use indexes

```
// list all collections
> db.getCollectionNames()
// delete part of document
db.users.update({"_id" : "testUser@domain.com"}, { $unset: {__Credentials:""}})

//list all databases
show dbs

// update multiple documents
db.users.update({}, { $unset: {Credentials:""}}, {multi: true})
```
