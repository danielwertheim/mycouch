using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace MyCouch.Serialization
{
    public class SerializationConfiguration
    {
        public JsonSerializerSettings Settings { get; protected set; }

        public SerializationConfiguration(IContractResolver contractResolver = null)
        {
            Settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = contractResolver ?? new SerializationContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
                Formatting = Formatting.None,
                DefaultValueHandling = DefaultValueHandling.Include,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };
            Settings.Converters.Add(new StringEnumConverter());
        }

        public virtual T ApplyConfigToWriter<T>(T writer) where T : JsonWriter
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

        public virtual T ApplyConfigToReader<T>(T reader) where T : JsonReader
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