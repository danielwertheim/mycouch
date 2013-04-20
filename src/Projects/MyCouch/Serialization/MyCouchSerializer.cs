using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MyCouch.Schemes;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    public class MyCouchSerializer : ISerializer
    {
        protected readonly IEntityAccessor EntityAccessor;
        protected readonly JsonSerializer Serializer;

        public MyCouchSerializer(IEntityAccessor entityAccessor)
        {
            EntityAccessor = entityAccessor;
            Serializer = CreateSerializer();
        }

        protected virtual JsonSerializer CreateSerializer()
        {
            return JsonSerializer.Create(CreateSettings());
        }

        protected virtual JsonSerializerSettings CreateSettings()
        {
            return new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = new SerializationContractResolver(EntityAccessor),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
                Formatting = Formatting.None,
                DefaultValueHandling = DefaultValueHandling.Include,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public virtual string Serialize<T>(T item) where T : class
        {
            var content = new StringBuilder();
            using (var textWriter = new StringWriter(content))
            {
                Serializer.Serialize(textWriter, item);
            }
            return content.ToString();
        }

        public virtual string SerializeEntity<T>(T entity) where T : class
        {
            var content = new StringBuilder();
            using (var textWriter = new StringWriter(content))
            {
                using (var jsonWriter = CreateEntityWriter(textWriter))
                {
                    jsonWriter.WriteDocHeaderFor(entity);
                    Serializer.Serialize(jsonWriter, entity);
                }
            }
            return content.ToString();
        }

        public virtual T Deserialize<T>(Stream data) where T : class
        {
            using (var reader = new StreamReader(data, Encoding.UTF8))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return Serializer.Deserialize<T>(jsonReader);
                }
            }
        }

        public virtual T Deserialize<T>(string data) where T : class
        {
            if (string.IsNullOrWhiteSpace(data))
                return null;

            using (var reader = new StringReader(data))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return Serializer.Deserialize<T>(jsonReader);
                }
            }
        }

        public virtual IEnumerable<T> Deserialize<T>(IEnumerable<string> data) where T : class
        {
            return data.Select(Deserialize<T>);
        }

        protected virtual SerializationEntityWriter CreateEntityWriter(TextWriter textWriter)
        {
            return new SerializationEntityWriter(textWriter);
        }
    }
}