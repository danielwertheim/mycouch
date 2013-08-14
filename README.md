# MyCouch #
Simple asynchronous CouchDb client for .Net - builds on top of the asynchronous HTTP client and uses JSON.Net to provide flexible serialization behavior. It tries to keep the domain language of CouchDb instead of bringing in generic repositories and other confusing stuff. MyCouch lets you work with raw JSON and/or entities/POCOS without requiring any implementation of interfaces, baseclasses etc. MyCouch provides you with some model conventions like injection of $doctype to the document. It is plug-gable. If you don't like some piece, then hook in your implementation instead.

## NuGet ##
MyCouch is distributed via NuGet. You can [find the package here](https://nuget.org/packages/MyCouch/).

## Documentation ##
The documentation is contained in the [project wiki](https://github.com/danielwertheim/mycouch/wiki).

## Get up and running with the source ##
The Sample has been written using Visual Studio 2012, targeting multiple platforms (.Net40, .Net45, Windows Store apps). .Net45 is the mainstream project. .Net4.0 uses Microsofts various portable class library (PCL) for adding missing BCL capabilities, the async HttpClient etc. to .Net4.0.

Please note. **No NuGet packages are checked in**. To get your project up and running you need to install the missing NuGet packages using the provided PowerShell script:

    ps:> .\setup-devenv.ps1

or

    cmd:> powershell -executionpolicy unrestricted .\setup-devenv.ps1

For the script to work, you need to have [the NuGet command line](http://nuget.codeplex.com/releases) `(NuGet.exe) registrered in the environment path`, or you need to tweak the script so it knows where it will find your NuGet.exe.

## How-to Contribute ##
This is described in the wiki, under: ["How-to Contribute"](https://github.com/danielwertheim/mycouch/wiki/how-to-contribute).

## Issues, questions, etc ##
So you have issues or questions... Great! That means someone is using it. Use the issues function here at the project page or contact me via mail: firstname@lastname.se; or Twitter: [@danielwertheim](https://twitter.com/danielwertheim)

## A word about the integration tests ##
They are written using MSTest (to be able to easily test the WinRT lib, supporting Windows Store apps). To get started you need to create a database `mycouchtests` and one user `mycouchtester` with password `p@ssword`. The user also must be allowed to create views in the database.

## License ##
The MIT License (MIT)

Copyright (c) 2013 Daniel Wertheim

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.