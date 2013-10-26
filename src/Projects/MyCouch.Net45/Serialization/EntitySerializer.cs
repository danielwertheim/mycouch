using System.IO;
using MyCouch.Serialization.Writers;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    public class EntitySerializer : DefaultSerializer, IEntitySerializer
    {
        public EntitySerializer(SerializationConfiguration configuration) : base(configuration)
        {
        }

        protected override JsonTextWriter CreateWriterFor<T>(TextWriter w)
        {
            return new EntityJsonWriter(typeof(T), w);
        }
    }
}