using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EnsureThat;
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
            var content = new StringBuilder(256);
            using (var stringWriter = new StringWriter(content, MyCouchRuntime.FormatingCulture.NumberFormat))
            {
                using (var jsonWriter = Configuration.ApplyConfigToWriter(CreateWriterFor<T>(stringWriter)))
                {
                    InternalSerializer.Serialize(jsonWriter, item);
                }
            }
            return content.ToString();
        }

        protected virtual string SerializeValue<T>(T value)
        {
            var content = new StringBuilder(16);
            using (var stringWriter = new StringWriter(content, MyCouchRuntime.FormatingCulture.NumberFormat))
            {
                using (var jsonWriter = Configuration.ApplyConfigToWriter(CreateWriterFor(stringWriter)))
                {
                    InternalSerializer.Serialize(jsonWriter, value);
                }
            }
            return content.ToString();
        }

        protected virtual JsonTextWriter CreateWriterFor(TextWriter writer)
        {
            return new JsonTextWriter(writer);
        }

        protected virtual JsonTextWriter CreateWriterFor<T>(TextWriter writer)
        {
            return new JsonTextWriter(writer);
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

        protected virtual JsonTextReader CreateReaderFor(TextReader reader)
        {
            return new DocumentJsonReader(reader);
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

        public virtual string ToJson(object value)
        {
            if (value == null)
                return null;

            if (value is string)
                return string.Format("\"{0}\"", value as string);

            if (value is Enum)
                return string.Format("\"{0}\"", value);

            if (value is Array)
                return ToJsonArray((object[])value);

            if (value is bool)
                return ToJson((bool)value);

            if (value is int)
                return ToJson((int)value);

            if (value is long)
                return ToJson((long)value);

            if (value is float)
                return ToJson((float)value);

            if (value is double)
                return ToJson((double)value);

            if (value is decimal)
                return ToJson((decimal)value);

            if (value is DateTime)
                return ToJson((DateTime)value);

            return SerializeValue(value);
        }

        public virtual string ToJson(bool value)
        {
            return value.ToString().ToLower();
        }

        public virtual string ToJson(int value)
        {
            return value.ToString(MyCouchRuntime.FormatingCulture);
        }

        public virtual string ToJson(long value)
        {
            return value.ToString(MyCouchRuntime.FormatingCulture);
        }

        public virtual string ToJson(float value)
        {
            return value.ToString(MyCouchRuntime.FormatingCulture);
        }

        public virtual string ToJson(double value)
        {
            return value.ToString(MyCouchRuntime.FormatingCulture);
        }

        public virtual string ToJson(decimal value)
        {
            return value.ToString(MyCouchRuntime.FormatingCulture);
        }

        public virtual string ToJson(DateTime value)
        {
            return SerializeValue(value);
        }

        public virtual string ToJsonArray<T>(IEnumerable<T> value)
        {
            return string.Format("[{0}]", string.Join(",", value.Select(v => ToJson(v))));
        }
    }
}