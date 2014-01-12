using System.IO;
using Newtonsoft.Json;

namespace MyCouch.Serialization.Writers
{
    public class DocumentJsonWriter : JsonTextWriter
    {
        public DocumentJsonWriter(TextWriter textWriter) : base(textWriter) { }

        public override void WriteNull()
        {
            base.WriteRaw(string.Empty);
        }
    }
}