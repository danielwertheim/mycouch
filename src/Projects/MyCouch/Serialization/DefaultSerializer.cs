using System.IO;
using System.Text;
using EnsureThat;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyCouch.Serialization
{
    public class DefaultSerializer : ISerializer
    {
        protected readonly JsonSerializerSettings Settings;
        protected readonly JsonSerializer InternalSerializer;

        public DefaultSerializer() : this(new SerializationContractResolver()) { }

        protected DefaultSerializer(IContractResolver contractResolver)
        {
            Ensure.That(contractResolver, "contractResolver").IsNotNull();

            Settings = CreateDefaultSettings(contractResolver);
            InternalSerializer = JsonSerializer.Create(Settings);
        }

        protected JsonSerializerSettings CreateDefaultSettings(IContractResolver contractResolver)
        {
            return new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = contractResolver,
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
            using (var stringWriter = new StringWriter(content))
            {
                using (var jsonWriter = ConfigureJsonWriter(new JsonTextWriter(stringWriter)))
                {
                    InternalSerializer.Serialize(jsonWriter, item);
                }
            }
            return content.ToString();
        }

        public virtual T Deserialize<T>(string data) where T : class
        {
            if (string.IsNullOrWhiteSpace(data))
                return null;

            using (var reader = new StringReader(data))
            {
                using (var jsonReader = ConfigureJsonReader(new JsonTextReader(reader)))
                {
                    return InternalSerializer.Deserialize<T>(jsonReader);
                }
            }
        }

        public virtual T Deserialize<T>(Stream data) where T : class
        {
            if (data == null || data.Length < 1)
                return null;

            using (var reader = new StreamReader(data, MyCouchRuntime.DefaultEncoding))
            {
                using (var jsonReader = ConfigureJsonReader(new JsonTextReader(reader)))
                {
                    return InternalSerializer.Deserialize<T>(jsonReader);
                }
            }
        }

        protected virtual T ConfigureJsonWriter<T>(T writer) where T : JsonTextWriter
        {
            writer.Culture = Settings.Culture;
            writer.DateFormatHandling = Settings.DateFormatHandling;
            writer.DateFormatString = Settings.DateFormatString;
            writer.DateTimeZoneHandling = Settings.DateTimeZoneHandling;
            writer.FloatFormatHandling = Settings.FloatFormatHandling;
            writer.Formatting = Settings.Formatting;
            writer.StringEscapeHandling = Settings.StringEscapeHandling;

            return writer;
        }

        protected virtual T ConfigureJsonReader<T>(T reader) where T : JsonTextReader
        {
            reader.Culture = Settings.Culture;
            reader.DateParseHandling = Settings.DateParseHandling;
            reader.DateTimeZoneHandling = Settings.DateTimeZoneHandling;
            reader.FloatParseHandling = Settings.FloatParseHandling;
            reader.MaxDepth = Settings.MaxDepth;
            
            return reader;
        }
    }
}