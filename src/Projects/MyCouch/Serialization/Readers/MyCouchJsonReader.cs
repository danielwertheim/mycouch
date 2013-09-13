using System.IO;
using Newtonsoft.Json;

namespace MyCouch.Serialization.Readers
{
    public class MyCouchJsonReader : JsonTextReader
    {
        public MyCouchJsonReader(TextReader reader) : base(reader)
        {
        }
    }
}
