using System;
using System.IO;
using System.Text;
using EnsureThat;
using MyCouch.Serialization.Readers;
using MyCouch.Serialization.Writers;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    public class DefaultSerializer : ISerializer
    {
        protected readonly SerializationConfiguration Configuration;
        protected readonly JsonSerializer InternalSerializer;

        public DefaultSerializer(SerializationConfiguration configuration)
        {
            Ensure.That(configuration, "configuration").IsNotNull();

            Configuration = configuration;
            InternalSerializer = JsonSerializer.Create(Configuration.Settings);
        }

        public virtual string Serialize<T>(T item) where T : class
        {
            var content = new StringBuilder();
            using (var stringWriter = new StringWriter(content))
            {
                using (var jsonWriter = Configuration.ApplyConfigToWriter(CreateWriterFor<T>(stringWriter)))
                {
                    InternalSerializer.Serialize(jsonWriter, item);
                }
            }
            return content.ToString();
        }

        protected virtual JsonTextWriter CreateWriterFor<T>(TextWriter w)
        {
            return new MyCouchJsonWriter(w);
        }

        public virtual T Deserialize<T>(string data) where T : class
        {
            if (string.IsNullOrWhiteSpace(data))
                return null;

            using (var sr = new StringReader(data))
            {
                using (var jsonReader = Configuration.ApplyConfigToReader(CreateReaderFor(sr)))
                {
                    return InternalSerializer.Deserialize<T>(jsonReader);
                }
            }
        }

        public virtual T Deserialize<T>(Stream data) where T : class
        {
            if (data == null || data.Length < 1)
                return null;

            using (var sr = new StreamReader(data, MyCouchRuntime.DefaultEncoding))
            {
                using (var jsonReader = Configuration.ApplyConfigToReader(CreateReaderFor(sr)))
                {
                    return InternalSerializer.Deserialize<T>(jsonReader);
                }
            }
        }

        protected virtual JsonTextReader CreateReaderFor(TextReader r)
        {
            return new MyCouchJsonReader(r);
        }

        public virtual void Populate<T>(T item, Stream data) where T : class
        {
            if (data == null || (data.CanSeek && data.Length < 1))
                return;

            using (var sr = new StreamReader(data, MyCouchRuntime.DefaultEncoding))
            {
                using (var jsonReader = Configuration.ApplyConfigToReader(CreateReaderFor(sr)))
                {
                    InternalSerializer.Populate(jsonReader, item);
                }
            }
        }
    }
}