using System.IO;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    public class DeserializationJsonWriter : JsonTextWriter
    {
        public DeserializationJsonWriter(TextWriter textWriter) : base(textWriter) { }

        public override void WriteNull()
        {
            base.WriteRaw(string.Empty);
        }
    }
}