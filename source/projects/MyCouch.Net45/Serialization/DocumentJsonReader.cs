using System.IO;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    public class DocumentJsonReader : JsonTextReader
    {
        public DocumentJsonReader(TextReader reader) : base(reader)
        {
        }
    }
}
