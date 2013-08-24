using System;
using System.IO;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    public delegate JsonTextReader JsonReaderFactory(Type docType, TextReader writer);
}