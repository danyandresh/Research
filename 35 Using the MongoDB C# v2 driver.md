#Using the MongoDB C# v2 driver
MongoDB.Bson, MongoDB.Driver and MongoDB.Driver.Core are the only packages for version 2.2

packages required:

- `MongoDB.Bson`
- `MongoDB.Driver`
- `MongoDB.Driver.Core`

##context
```
public class MongoContext
{
    public IMongoDatabase Database;
    
    public MongoContext()
    {
        var client = new MongoClient(Settings.Default.MongoConnectionString);
        Database = client.GetDatabase(Settings.Default.MongoDatabase);
    }
    
    public IMongoCollection<_model_> Models => Database.GetCollection<_model_>("_models_");
}
```

get build info

```
var buildInfoCommand = new BsonDocument("buildinf", 1);

var buildInfo = Context.Database.RunCommand<BsonDocument>(buildInfoCommand);
buildInfo = await Context.Database.RunCommandAsync<BsonDocument>(buildInfoCommand);

return Content(buildInfo.ToJson(), "application/json");
```

https://docs.mongodb.com/ecosystem/drivers/csharp/

http://mongodb.github.io/mongo-csharp-driver/2.2/getting_started/quick_tour/

http://mongodb.github.io/mongo-csharp-driver/2.2/reference/

##finding all documents

```
collection.Find(new BsonDocument()).ToList();
collection.Find(new BsonDocument()).ToListAsync();
collection.Find(new BsonDocument()).ForEachAsync();
```

http://github.com/mongodb/mongo-csharp-driver

##filtering
```
var fiterDefinition = Builders<_model_>.Filter.Where(r=>r.prop >= value); 
```

##rendering
returns bson document from the query structure
```
var serializerRegistry = BsonSerializer.SerializerRegistry;
var serializer = BsonSerializer.SerializerRegistry.GetSerializer<_model_>();

query.Render(serializer, serializerRegistry);
```

##combining filters
using `&`
```
filterDefinition &= Builders<Rental>.Filter.Lte(r => r.Price, filters.PriceLimit);
```

##sorting
```
collection
    .Find(filterDefinition)
    .Sort(m => m.Property)
    .ToList();
```

##insert
```
collection
    .InsertOne(newModel);
```
##find
```
collection.Find(m => m.Id == id).FirstOrDefault();
```

##update
```
collection.ReplaceOne(m => m.Id == id, newModel);

var modificationUpdate = Builders<Rental>
    .Update
    .Push(r => r.Array, newItem)
    .Set(r => r.Prop, newValue);
collection.ReplaceOne(m => m.Id == id, modificationUpdate);

var options = new UpdateOptions
    {
        IsUpsert = true,
    };
await collection.ReplaceOneAsync(m => m.Id == id, newModel, options);
```

##delete
```
collection.DeleteOne(m => m.Id == id);
```

###projections
```
collection
    .Find(filter)
    .Project(m => new ViewModel
        {
            Id = r.Id,
            <other fields to be included in projection>
        }
    )
```

##profiling
```
db.setProfilingLevel(2);

db.system.profile.find().
```

##eventing
###listening for the `CommandStartedEvent`
the driver is polling on the server to check status, so it can hook to these events
```
var mongoUrl = "http://localhost";

public class MongoContext
{
    public IMongoDatabase Database;
    
    public MongoContext()
    {
        var settings = MongoClientSettings.FromUrl(new MongoUrl(mongoUrl));
        settings.ClusterConfigurator = builder => builder.Subscribe<CommandStartedEvent>(started => {});
        
        var client = new MongoClient(settings);
        Database = client.GetDatabase(Settings.Default.MongoDatabase);
    }
    
    public IMongoCollection<_model_> Models => Database.GetCollection<_model_>("_models_");
}
```

####log4net subscriber
```
public class Log4NetMongoEvents : IEventSubscriber
{
    private ReflectionEventSubscriber _subscriber;
    
    public Log4NetMongoEvents()
    {
        _subscriber = new ReflectionEventSubscriber(this);
    }
    
    public bool TryGetEventHandler<TEvent>(out Action<TEvent> handler)
    {
        return _subscriber.TryGetEventHandler(out handler);
    }    
    
    public void Handle(CommandStartedEvent started)
    {
        CommandStartedLog.Info(
        {
            started.Command,
            started.CommandName,
            started.ConnectionId,
            started.DatabaseNamespace,
            started.OperationId,
            started.RequestId
        });
    }
```

```
<log4net>
    <appender name="FileAppender" type="log4net.Appender.FileAppender">
        <file value="${TMP}/log.txt" />
        <appendToFile value="true" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%date [%thread%] %-5level %logger [%property{NDC}] - %message%newline"
        </layout>
    </appender>
    <root>
        <level value="ALL" />
        <appender-ref ref="FileAppender" />
    </root>
</log4net>
```

##joins, LINQ and the aggregation framework
server-side reduce
```
var mapStatePopulations = function(){
    emit(this.state, this.pop);
};

var reduceStatePopulation = function(state, zipCodePopulations){
    return Array.sum(zipCodePopulations);
};

db.zips.mapReduce(
    mapStatePopulations,
    reducestatePopulation,
    { out: "statePopulations"})
    
db.getCollection("statePopulations").find({}).sort({ value: 1})

db.zips.aggregate([
    {$group: { _id: "$state", population: { $sum: "$pop" }}},
    {$match: { population: { $lte:1e6}}},
    {$sort: {population: 1}}
])
```

###linq
```
collection.AsQueryable() // IMongoQueryable
    .Select(projection)
    .OrderBy(condition)
    .ThenBy(condition)
    .ToListAsync()

```

###fluent aggregate
```
collection.Aggregate()
    .Project(r => new {r.prop})
    .Group(r => r.prop1, g => new {g.Key, Count = g.Count()})
```

###joins with $lookup, server side
```
collection.Aggregate()
    .Lookup<t1, t2, BsonDocument>(collection2,
        e1 => e1.prop1
        e2 => e2.prop2,
        r => r["joinedT2Array"])
    .ToList()
    
collection.Aggregate()
    .Lookup<t1, t2, tResult>(collection2,
        e1 => e1.prop1
        e2 => e2.prop2,
        eR => eRr.joinedT2Array)
    .ToList()
    
```

##gridfs for storing files
`HttpPostedFileBase`

```
var bucket = new GridFSBucket(context.Database);
var options = new GridFSUploadOptions
{
    Metadata = new BsonDocument("contentType", file.ContentType)
};

var imageId = await bucket.UploadFromStreamAsync(file.FileName, file.InputStream, options);

```

###download file
``` 
var stream = bucket.OpenDownloadStream(new ObjectId(id)) //will throw exception is no file is found GirdFSFileNotFOundException
var contentType = stream.FileInfo.ContentType
    ?? stream.FIleInfo.Metadata["contentType"].AsString;
    
return FIle(stream, contentType);

```

###delete file
```
bucket.DelteAsync(new ObjectId(fileId));
```
