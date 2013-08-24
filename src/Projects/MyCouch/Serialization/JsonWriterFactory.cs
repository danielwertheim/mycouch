using System;
using System.IO;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    public delegate JsonTextWriter JsonWriterFactory(Type docType, TextWriter writer);
}