# MyCouch #
The asynchronous CouchDb and Cloudant client for .Net - builds on top of the asynchronous HTTP client and uses JSON.Net to provide flexible serialization behaviour. It tries to keep the domain language of CouchDb instead of bringing in generic repositories and other confusing stuff. MyCouch lets you work with raw JSON and/or entities/POCOS without requiring any implementation of interfaces, baseclasses etc. MyCouch provides you with some model conventions like injection of `$doctype` to the document. It is plug-gable. If you don't like some piece, then hook in your implementation instead.

MyCouch is not an official client created by Cloudant.

## NuGet ##
MyCouch is distributed via NuGet. You can [find the package here](https://nuget.org/packages/MyCouch/). But basically, in a .Net4.0, .Net4.5 or Windows Store app project, open up the Package manager console, and invoke:

    pm:> install-package mycouch

or if you also want some [Cloudant](http://cloudant.com) specific features like [Lucene searches](https://cloudant.com/for-developers/search/):

	pm:> install-package mycouch.cloudant

**Please note!** Some users with old versions of NuGet has reported that dependencies to `Ensure.That` might not be resolved. The solution is to update NuGet.

## Documentation ##
The documentation is contained in the [project wiki](https://github.com/danielwertheim/mycouch/wiki).

## Trello board
A [public Trello board](https://trello.com/b/wuDUldwD/mycouch-main) is used instead of a "roadmap".

## Get up and running with the source ##
The Sample has been written using Visual Studio 2012, targeting multiple platforms (.Net40, .Net45, Windows Store apps). .Net45 is the mainstream project. .Net4.0 uses Microsofts various portable class library (PCL) for adding missing BCL capabilities, the async HttpClient etc. to .Net4.0.

Please note. **No NuGet packages are checked in**. If you are using the latest version of NuGet (v2.7.1+) you should be able to just build and the packages will be restored. If this does not work, you could install the missing NuGet packages using the provided PowerShell script:

    ps:> .\setup-devenv.ps1

or

    cmd:> powershell -executionpolicy unrestricted .\setup-devenv.ps1

For the script to work, you need to have [the NuGet command line](http://nuget.codeplex.com/releases) `(NuGet.exe) registrered in the environment path`, or you need to tweak the script so it knows where it will find your NuGet.exe.

## A word about the integration tests ##
They are written using xUnit. To get started you need to create a database `mycouchtests` and one user `mycouchtester` with password `p@ssword`. The user also must be allowed to create views in the database.

**Please note**, that if you run the tests using the `xUnit plugin` for `ReSharper` or using the `xUnit console`, you will get a couple of failing UnitTests for the Windows store project. This is a **bug with xUnit**. It does not load and execute the Windows store test project in its own app-container as it does when you execute them via Visual Studios test explorer.

### Test environments ###
There is an external dependency for getting up and running with the integration tests (not the unit tests). In the repo, there's a folder in the root named `env`. In there there's a [ScriptCS](http://scriptcs.net) script `server.csx`. It will start a NancyFX web server that serves the integration tests with test environment settings.

The first time you set this up, ensure you have ScriptCS installed and then in the `env` folder just type: `scriptcs -install scriptcs.nancy`. When done, delete the generated `pacakges.config` file and just run the `start.bat` file. The actual configuration is specified in JSON-files under `env\data\[testenvironment].json`.

## How-to Contribute ##
This is described in the wiki, under: ["How-to Contribute"](https://github.com/danielwertheim/mycouch/wiki/how-to-contribute).

## Issues, questions, etc ##
So you have issues or questions... Great! That means someone is using it. Use the issues function here at the project page or contact me via mail: firstname@lastname.se; or Twitter: [@danielwertheim](https://twitter.com/danielwertheim)

## License ##
The MIT License (MIT)

Copyright (c) 2013 Daniel Wertheim

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
