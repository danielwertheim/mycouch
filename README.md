# MyCouch #
The asynchronous CouchDb and Cloudant client for .Net - builds on top of the asynchronous HTTP client and uses JSON.Net to provide flexible serialization behaviour. It tries to keep the domain language of CouchDb instead of bringing in generic repositories and other confusing stuff. MyCouch lets you work with raw JSON and/or entities/POCOS without requiring any implementation of interfaces, baseclasses etc. MyCouch provides you with some model conventions like injection of `$doctype` to the document. It is plug-gable. If you don't like some piece, then hook in your implementation instead.

## NOTE! ##
**It's your data.** Ensure to **test against isolated test-environments and test-accounts first** e.g. a separate Cloudant account, specific CouchDb instances etc.

## Documentation, Roadmap, Milestones & Issues ##
The documentation is contained in the [project wiki](https://github.com/danielwertheim/mycouch/wiki).

The ["Issues list" here on GitHub](https://github.com/danielwertheim/mycouch/issues) is used for tracking issues and headings.

## More MyCouch projects ##
[MyCouch.AspNet.Identity](https://github.com/danielwertheim/mycouch.aspnet.identity) - an ASP.Net identity provider for CouchDb and Cloudant

## NuGet ##
MyCouch is distributed via NuGet.

- [CouchDb package](https://nuget.org/packages/MyCouch/)
- [Cloudant package](https://nuget.org/packages/MyCouch.Cloudant/)

But basically, in a .Net4.0, .Net4.5 or Windows Store app project, open up the Package manager console, and invoke:

    pm:> install-package mycouch

or if you also want some [Cloudant](http://cloudant.com) specific features like [Lucene searches](https://cloudant.com/for-developers/search/):

	pm:> install-package mycouch.cloudant

## Quick sample ##

```csharp
using(var client = new MyCouchClient("http://localhost:5984/mydb"))
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

## Get up and running with the source ##
- For .Net4.0 and .Net4.5, Visual Studio 2012 is needed.
- For Windows store 8.0, currently not included. But let me know and I will assist or fix it.
- For Windows store 8.1, Visual Studio 2013 is needed.

Please note. **No NuGet packages are checked in**. If you are using the latest version of NuGet (v2.7.1+) **you should be able to just build and the packages will be restored**. If this does not work, you could install the missing NuGet packages using a simple PowerShell script as covered here: http://danielwertheim.se/2013/08/12/nuget-restore-powershell-vs-rake/

## A word about the integration tests ##
They are written using **xUnit**. To get started you need to create a database `mycouchtests` and one user `mycouchtester` with password `p@ssword`. The user also must be allowed to create views in the database.

### Test environments ###
The project `MyCouch.TestServer` is a small self-hosted Nancy server that is used to serve e.g. test environment configurations to the integration tests. Just ensure there's a folder called `env\data` and that it contains three JSON-files (Cloudant is only needed if you want to run thoose tests). Read more about this in `env\README.md`.

The `MyCouch.TestServer` project **is not being built** in `DEBUG` nor for `RELEASE`. Hence the first time you need to explicitly e.g rebuild it via right clicking on it in the Solution Explorer.

## How-to Contribute ##
This is described in the wiki, under: ["How-to Contribute"](https://github.com/danielwertheim/mycouch/wiki/how-to-contribute).

## Issues, questions, etc ##
So you have issues or questions... Great! That means someone is using it. Use the issues function here at the project page or contact me via mail: firstname@lastname.se; or Twitter: [@danielwertheim](https://twitter.com/danielwertheim)

## License ##
The MIT License (MIT)

Copyright (c) 2014 Daniel Wertheim

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
