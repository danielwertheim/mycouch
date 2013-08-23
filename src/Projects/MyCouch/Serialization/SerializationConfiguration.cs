using EnsureThat;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyCouch.Serialization
{
    public class SerializationConfiguration
    {
        public JsonSerializerSettings Settings { get; protected set; }

        public SerializationConfiguration(IContractResolver contractResolver)
        {
            Ensure.That(contractResolver, "contractResolver").IsNotNull();

            Settings = new JsonSerializerSettings
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

        public virtual T ApplyToWriter<T>(T writer) where T : JsonTextWriter
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

        public virtual T ApplyToReader<T>(T reader) where T : JsonTextReader
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