using System.IO;
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

        public virtual void PopulateSingleDocumentResponse<T>(T response, Stream data) where T : SingleDocumentResponse
        {
            using (var sr = new StreamReader(data))
            {
                using (var jr = new JsonTextReader(sr))
                {
                    while (jr.Read())
                    {
                        if (jr.TokenType == JsonToken.PropertyName)
                        {
                            var propName = jr.Path;
                            if (propName == "id")
                            {
                                if (!jr.Read())
                                    break;
                                response.Id = jr.Value.ToString();
                                continue;
                            }

                            if (propName == "rev")
                            {
                                if (!jr.Read())
                                    break;
                                response.Rev = jr.Value.ToString();
                                continue;
                            }
                        }
                    }
                }
            }
        }

        public virtual void PopulateViewQueryResponse<T>(ViewQueryResponse<T> response, Stream data) where T : class
        {
            using (var sr = new StreamReader(data))
            {
                using (var jr = new JsonTextReader(sr))
                {
                    while (jr.Read())
                    {
                        if (jr.TokenType == JsonToken.PropertyName)
                        {
                            var propName = jr.Path;
                            if (propName == "total_rows")
                            {
                                if (!jr.Read())
                                    break;
                                response.TotalRows = (long)jr.Value;
                                continue;
                            }

                            if (propName == "offset")
                            {
                                if (!jr.Read())
                                    break;
                                response.OffSet = (long)jr.Value;
                                continue;
                            }

                            if (propName == "rows")
                            {
                                if (!jr.Read())
                                    break;

                                response.Rows = Serializer.Deserialize<ViewQueryResponse<T>.Row[]>(jr);
                            }
                        }
                    }
                }
            }
        }

        public virtual void PopulateFailedResponse<T>(T response, Stream data) where T : Response
        {
            using (var sr = new StreamReader(data))
            {
                using (var jr = new JsonTextReader(sr))
                {
                    while (jr.Read())
                    {
                        if (jr.TokenType == JsonToken.PropertyName)
                        {
                            var propName = jr.Path;
                            if (propName == "error")
                            {
                                if (!jr.Read())
                                    break;
                                response.Error = jr.Value.ToString();
                                continue;
                            }

                            if (propName == "reason")
                            {
                                if (!jr.Read())
                                    break;
                                response.Reason = jr.Value.ToString();
                                continue;
                            }
                        }
                    }
                }
            }
        }

        protected virtual SerializationEntityWriter CreateEntityWriter(TextWriter textWriter)
        {
            return new SerializationEntityWriter(textWriter);
        }
    }
}