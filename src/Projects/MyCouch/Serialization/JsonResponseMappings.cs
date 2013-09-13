using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    public class JsonResponseMappings : Dictionary<string, Action<JsonReader>> { }
}