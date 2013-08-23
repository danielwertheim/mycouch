using System.IO;
using System.Text;
using MyCouch.Serialization;
using Newtonsoft.Json.Serialization;

namespace MyCouch.Rich.Serialization
{
    public class RichSerializer : DefaultSerializer
    {
        public RichSerializer(IContractResolver contractResolver) 
            : base(contractResolver)
        {
        }

        public override string Serialize<T>(T item)
        {
            var content = new StringBuilder();
            using (var stringWriter = new StringWriter(content))
            {
                using (var jsonWriter = ConfigureJsonWriter(new SerializationJsonWriter(stringWriter)))
                {
                    jsonWriter.WriteDocHeaderFor(item);
                    InternalSerializer.Serialize(jsonWriter, item);
                }
            }
            return content.ToString();
        }
    }
}