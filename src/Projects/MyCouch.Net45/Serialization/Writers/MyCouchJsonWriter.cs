using System.IO;
using Newtonsoft.Json;

namespace MyCouch.Serialization.Writers
{
    public class MyCouchJsonWriter : JsonTextWriter
    {
        public MyCouchJsonWriter(TextWriter textWriter) : base(textWriter) { }

        public override void WriteNull()
        {
            base.WriteRaw(string.Empty);
        }
    }
}