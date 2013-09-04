using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyCouch.Serialization
{
    public class SerializationConfiguration
    {
        public delegate JsonTextReader JsonReaderFactory(Type docType, TextReader writer);
        public delegate JsonTextWriter JsonWriterFactory(Type docType, TextWriter writer);

        public JsonSerializerSettings Settings { get; protected set; }
        public JsonReaderFactory ReaderFactory { get; set; }
        public JsonWriterFactory WriterFactory { get; set; }

        public SerializationConfiguration(IContractResolver contractResolver = null)
        {
            ReaderFactory = DefaultReaderFactory;
            WriterFactory = DefaultWriterFactory;

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
        }

        protected virtual JsonTextReader DefaultReaderFactory(Type docType, TextReader reader)
        {
            return ApplyConfigToReader(new JsonTextReader(reader));
        }

        protected virtual JsonTextWriter DefaultWriterFactory(Type docType, TextWriter writer)
        {
            return ApplyConfigToWriter(new JsonTextWriter(writer));
        }

        public virtual T ApplyConfigToWriter<T>(T writer) where T : JsonTextWriter
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

        public virtual T ApplyConfigToReader<T>(T reader) where T : JsonTextReader
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