using System.IO;
using System.Text;
using MyCouch.Serialization;
using Newtonsoft.Json.Serialization;

namespace MyCouch.Rich.Serialization
{
    public class RichSerializer : DefaultSerializer, IRichSerializer
    {
        public RichSerializer(IContractResolver contractResolver) 
            : base(contractResolver)
        {
        }

        public virtual string SerializeEntity<T>(T entity) where T : class
        {
            var content = new StringBuilder();
            using (var stringWriter = new StringWriter(content))
            {
                using (var jsonWriter = ConfigureJsonWriter(new SerializationJsonWriter(stringWriter)))
                {
                    jsonWriter.WriteDocHeaderFor(entity);
                    InternalSerializer.Serialize(jsonWriter, entity);
                }
            }
            return content.ToString();
        }
    }
}