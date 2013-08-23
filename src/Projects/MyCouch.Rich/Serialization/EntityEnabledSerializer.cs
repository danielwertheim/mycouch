using System.IO;
using System.Text;
using MyCouch.Serialization;

namespace MyCouch.Rich.Serialization
{
    public class EntityEnabledSerializer : DefaultSerializer
    {
        public EntityEnabledSerializer(SerializationConfiguration configuration) : base(configuration)
        {
        }

        public override string Serialize<T>(T item)
        {
            var content = new StringBuilder();
            using (var stringWriter = new StringWriter(content))
            {
                using (var jsonWriter = Configuration.ApplyToWriter(new SerializationJsonWriter(stringWriter)))
                {
                    jsonWriter.WriteDocHeaderFor(item);
                    InternalSerializer.Serialize(jsonWriter, item);
                }
            }
            return content.ToString();
        }
    }
}