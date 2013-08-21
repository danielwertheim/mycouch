using System;
using System.IO;
using System.Text;
using MyCouch.Schemes;
using MyCouch.Serialization;

namespace MyCouch.Rich.Serialization
{
    public class RichSerializer : DefaultSerializer, IRichSerializer
    {
        public RichSerializer(Func<IEntityReflector> entityReflectorFn) 
            : base(new RichSerializationContractResolver(entityReflectorFn))
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