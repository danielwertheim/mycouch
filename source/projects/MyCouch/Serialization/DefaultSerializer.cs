using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EnsureThat;
using MyCouch.EntitySchemes;
using MyCouch.Serialization.Meta;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    public class DefaultSerializer : ISerializer
    {
        protected readonly SerializationConfiguration Configuration;
        protected readonly JsonSerializer InternalSerializer;
        protected readonly IDocumentSerializationMetaProvider DocumentMetaProvider;
        protected readonly IEntityReflector EntityReflector;

        public DefaultSerializer(SerializationConfiguration configuration, IDocumentSerializationMetaProvider documentMetaProvider, IEntityReflector entityReflector = null)
        {
            EnsureArg.IsNotNull(configuration, nameof(configuration));
            EnsureArg.IsNotNull(documentMetaProvider, nameof(documentMetaProvider));

            Configuration = configuration;
            DocumentMetaProvider = documentMetaProvider;
            EntityReflector = entityReflector;
            InternalSerializer = JsonSerializer.Create(Configuration.Settings);
        }

        protected virtual JsonTextWriter CreateWriterFor<T>(TextWriter writer)
        {
            return CreateWriterFor(typeof(T), writer);
        }

        protected virtual JsonTextWriter CreateWriterFor(Type docType, TextWriter writer)
        {
            if (EntityReflector == null)
                return CreateWriter(writer);

            var documentMeta = DocumentMetaProvider.Get(docType);

            return documentMeta == null
                ? CreateWriter(writer)
                : new DocumentJsonWriter(writer, documentMeta, Configuration.Conventions, EntityReflector);
        }

        protected virtual JsonTextWriter CreateWriter(TextWriter writer)
        {
            return new JsonTextWriter(writer) { CloseOutput = false };
        }

        protected virtual JsonTextReader CreateReaderFor<T>(TextReader reader)
        {
            return CreateReader(typeof(T), reader);
        }

        protected virtual JsonTextReader CreateReader(Type type, TextReader reader)
        {
            return EntityReflector == null
                ? CreateReader(reader)
                : new DocumentJsonReader(reader, type, EntityReflector);
        }

        protected virtual JsonTextReader CreateReader(TextReader reader)
        {
            return new JsonTextReader(reader) { CloseInput = false };
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
            using (var stringWriter = new StringWriter(content, MyCouchRuntime.FormatingCulture))
            {
                using (var jsonWriter = Configuration.ApplyConfigToWriter(CreateWriter(stringWriter)))
                {
                    InternalSerializer.Serialize(jsonWriter, value);
                }
            }
            return content.ToString();
        }

        public virtual T Deserialize<T>(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return default(T);

            using (var sr = new StringReader(data))
            {
                using (var jsonReader = Configuration.ApplyConfigToReader(CreateReaderFor<T>(sr)))
                {
                    return InternalSerializer.Deserialize<T>(jsonReader);
                }
            }
        }

        public virtual T Deserialize<T>(Stream data)
        {
            if (StreamIsEmpty(data))
                return default(T);

            using (var sr = new StreamReader(data, MyCouchRuntime.DefaultEncoding))
            {
                using (var jsonReader = Configuration.ApplyConfigToReader(CreateReaderFor<T>(sr)))
                {
                    return InternalSerializer.Deserialize<T>(jsonReader);
                }
            }
        }

        public virtual void Populate<T>(T item, Stream data) where T : class
        {
            if (StreamIsEmpty(data))
                return;

            using (var sr = new StreamReader(data, MyCouchRuntime.DefaultEncoding))
            {
                using (var jsonReader = Configuration.ApplyConfigToReader(CreateReaderFor<T>(sr)))
                {
                    if (!jsonReader.Read())
                        return;

                    InternalSerializer.Populate(jsonReader, item);
                }
            }
        }

        public virtual void Populate<T>(T item, string data) where T : class
        {
            if (string.IsNullOrWhiteSpace(data))
                return;

            using (var sr = new StringReader(data))
            {
                using (var jsonReader = Configuration.ApplyConfigToReader(CreateReaderFor<T>(sr)))
                {
                    InternalSerializer.Populate(jsonReader, item);
                }
            }
        }

        private bool StreamIsEmpty(Stream stream)
        {
            var stupidExpressionDueToThatLengthThrowsInSomeCases = stream == null || (stream.CanSeek && stream.Length == 0);

            return stupidExpressionDueToThatLengthThrowsInSomeCases;
        }

        public virtual string ToJson(object value)
        {
            if (value == null)
                return null;

            if (value is string)
                return JsonConvert.ToString(value as string, '"', Configuration.Settings.StringEscapeHandling);

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