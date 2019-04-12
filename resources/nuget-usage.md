## Quick sample - using Requests and Responses ##

```csharp
using(var client = new MyCouchClient("http://localhost:5984/", "mydb"))
{
    //POST with server generated id
    await client.Documents.PostAsync("{\"name\":\"Daniel\"}");

	//POST with client generated id - possible but wrong
    await client.Documents.PostAsync("{\"_id":\"someId", \"name\":\"Daniel\"}");

    //PUT for client generated id
    await client.Documents.PutAsync("someId", "{\"name\":\"Daniel\"}");

    //PUT for updates
    await client.Documents.PutAsync("someId", "docRevision", "{\"name\":\"Daniel Wertheim\"}");

	//PUT for updates with _rev in JSON
    await client.Documents.PutAsync("someId", "{\"_rev\": \"docRevision\", \"name\":\"Daniel Wertheim\"}");

    //Using entities
    var me = new Person {Id = "SomeId", Name = "Daniel"};
    await client.Entities.PutAsync(me);

    //Using anonymous entities
    await client.Entities.PostAsync(new { Name = "Daniel" });
}
```

```csharp
using(var client = new MyCouchServerClient("http://localhost:5984"))
{
    var r = await client.Replicator.ReplicateAsync(id, source, target);
}
```

## Quick sample - using MyCouchStore ##
```csharp
using(var store = new MyCouchStore("http://localhost:5984", "mydb"))
{
    var mySomething = await store.StoreAsync(new Something("foo", "bar", 42));

    var retrieved = await store.GetByIdAsync(mySomething.Id);

    var deleted = await store.DeleteAsync(mySomething.Id, mySomething.Rev);

    //... ... and so on... ...
}
```
