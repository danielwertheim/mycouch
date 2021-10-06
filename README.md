# MyCouch #
The asynchronous CouchDB client for .NET - builds on top of the asynchronous HTTP client and uses JSON.Net to provide flexible serialization behaviour. It tries to keep the domain language of CouchDB instead of bringing in generic repositories and other confusing stuff. MyCouch lets you work with raw JSON and/or entities/POCOS without requiring any implementation of interfaces, baseclasses etc. MyCouch provides you with some model conventions like injection of `$doctype` to the document.

**Multiple target frameworks:** `.NET Standard 1.1`, `.NET Standard 2.0`; using a .NET Standard project.

[![Build Status](https://dev.azure.com/daniel-wertheim/os/_apis/build/status/mycouch-CI?branchName=master)](https://dev.azure.com/daniel-wertheim/os/_build/latest?definitionId=7&branchName=master)
[![Nuget](https://img.shields.io/nuget/v/mycouch.svg)](https://www.nuget.org/packages/mycouch/)

The documentation is contained in the [project wiki](https://github.com/danielwertheim/mycouch/wiki).

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

## Integration tests
The `./.env` file and `./src/tests/IntegrationTests/integrationtests.local.ini` files are `.gitignored`. In order to create sample files of these, you can run:

```
. init-local-config.sh
```

### Docker-Compose
There's a `docker-compose.yml` file, that defines usage of a single node CouchDB over port `5984`. The `COUCHDB_USER` and `COUCHDB_PASSWORD` is configured via environment key `MyCouch_User` and `MyCouch_Pass`; which can either be specified via:

- Environment variable: `MyCouch_User` and `MyCouch_Pass`, e.g.:
```
MyCouch_User=sample_user
MyCouch_Pass=sample_password
```

- Docker Environment file `./.env` (`.gitignored`), e.g.:
```
MyCouch_User=sample_user
MyCouch_Pass=sample_password
```

### Test configuration
Credentials need to be provided, either via:

- Local-INI-file (`.gitignored`): `./src/tests/IntegrationTests/integrationtests.local.ini`, e.g.:
```
User=sample_user
Pass=sample_password
```

- Environment variables: `MyCouch_User` and `MyCouch_Pass`, e.g.:

```
MyCouch_User=sample_user
MyCouch_Pass=sample_password
```
