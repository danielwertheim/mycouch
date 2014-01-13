using System.Runtime.InteropServices;
﻿using System.Reflection;

#if DEBUG
[assembly: AssemblyProduct("MyCouch (Debug)")]
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyProduct("MyCouch (Release)")]
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyDescription("MyCouch - async .Net client for CouchDb and Cloudant.")]
[assembly: AssemblyCompany("Daniel Wertheim")]
[assembly: AssemblyCopyright("Copyright © 2014 Daniel Wertheim")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("0.20.0.*")]
[assembly: AssemblyFileVersion("0.20.0")]