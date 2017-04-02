# MyCouch #
The asynchronous CouchDB client for .NET - builds on top of the asynchronous HTTP client and uses JSON.Net to provide flexible serialization behaviour. It tries to keep the domain language of CouchDB instead of bringing in generic repositories and other confusing stuff. MyCouch lets you work with raw JSON and/or entities/POCOS without requiring any implementation of interfaces, baseclasses etc. MyCouch provides you with some model conventions like injection of `$doctype` to the document.

**.NET Standard 1.3**. MyCouch (after v5.0.0) is now built as a .NET Standard 1.3 library.

**MyCouch**

[![Nuget](https://img.shields.io/nuget/v/mycouch.svg)](https://www.nuget.org/packages/mycouch/)

## Documentation, Release Notes & Issues ##
The documentation is contained in the [project wiki](https://github.com/danielwertheim/mycouch/wiki).

The **Release Notes** is well worth a read for [news and breaking changes info](https://github.com/danielwertheim/mycouch/wiki/release-notes).

The ["Issues list" here on GitHub](https://github.com/danielwertheim/mycouch/issues) is used for tracking issues.

## More MyCouch projects ##
[MyCouch.AspNet.Identity](https://github.com/danielwertheim/mycouch.aspnet.identity) - an ASP.Net identity provider for CouchDB

## NuGet ##
MyCouch is distributed via NuGet.

- [CouchDB package](https://nuget.org/packages/MyCouch/)

So...

    pm:> install-package mycouch

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

## Issues, questions, etc ##
So you have issues or questions... Great! That means someone is using it. Use the issues function here at the project page or contact me via mail: firstname@lastname.se; or Twitter: [@danielwertheim](https://twitter.com/danielwertheim)