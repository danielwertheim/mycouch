using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MyCouch.Schemes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace MyCouch.Serialization
{
    public class MyCouchSerializer : ISerializer
    {
        protected readonly IEntityAccessor EntityAccessor;
        protected readonly JsonSerializer Serializer;
        protected readonly JsonSerializer Deserializer;

        public MyCouchSerializer(IEntityAccessor entityAccessor)
        {
            EntityAccessor = entityAccessor;
            Serializer = JsonSerializer.Create(CreateSettings(new SerializationContractResolver(EntityAccessor)));
            Deserializer = JsonSerializer.Create(CreateSettings(new DefaultContractResolver(true)));
        }

        protected virtual JsonSerializerSettings CreateSettings(IContractResolver contractResolver)
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
                    return Deserializer.Deserialize<T>(jsonReader);
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
                    return Deserializer.Deserialize<T>(jsonReader);
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
                        if (jr.TokenType != JsonToken.PropertyName) 
                            continue;
                        
                        var propName = jr.Value.ToString();
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

                            if (response is ViewQueryResponse<string>)
                                response.Rows = YieldViewQueryStringResponseRows<T>(jr).ToArray();
                            else if (response is ViewQueryResponse<string[]>)
                            {
                                //TODO: Fix
                            }
                            else
                                response.Rows = Deserializer.Deserialize<ViewQueryResponse<T>.Row[]>(jr);
                        }
                    }
                }
            }
        }

        protected IEnumerable<ViewQueryResponse<T>.Row> YieldViewQueryStringResponseRows<T>(JsonReader jr) where T : class
        {
            if (jr.TokenType != JsonToken.StartArray)
                yield break;

            var row = new ViewQueryResponse<T>.Row();
            var startDepth = jr.Depth;
            var sb = new StringBuilder();
            var hasMappedId = false;
            var hasMappedKey = false;
            var hasMappedValue = false;
            using (var sw = new StringWriter(sb))
            {
                using (var jw = new JsonTextWriter(sw))
                {
                    while (jr.Read() && !(jr.TokenType == JsonToken.EndArray && jr.Depth == startDepth))
                    {
                        if (jr.TokenType != JsonToken.PropertyName) 
                            continue;

                        var propName = jr.Value.ToString().ToLower();
                        if (propName == "id")
                        {
                            if (!jr.Read())
                                break;
                            row.Id = jr.Value.ToString();
                            hasMappedId = true;
                        }
                        else if (propName == "key")
                        {
                            if (!jr.Read())
                                break;
                            row.Key = jr.Value.ToString();
                            hasMappedKey = true;
                        }
                        else if (propName == "value")
                        {
                            if (!jr.Read())
                                break;
                            jw.WriteToken(jr, true);
                            row.Value = sb.ToString() as T;
                            sb.Clear();
                            hasMappedValue = true;
                        }
                        else
                            continue;

                        if (hasMappedId && hasMappedKey && hasMappedValue)
                        {
                            hasMappedId = hasMappedKey = hasMappedValue = false;
                            yield return row;
                            row = new ViewQueryResponse<T>.Row();
                        }
                    }
                }
            }
        }

        //protected IEnumerable<string> YieldViewQueryResponseRows(JsonReader jr)
        //{
        //    if(jr.TokenType != JsonToken.StartArray)
        //        yield break;

        //    var sb = new StringBuilder();
        //    using (var sw = new StringWriter(sb))
        //    {
        //        using (var jw = new JsonTextWriter(sw))
        //        {
        //            var startDepth = jr.Depth;
        //            while (jr.Read() && !(jr.TokenType == JsonToken.EndArray && jr.Depth == startDepth))
        //            {
        //                jw.WriteToken(jr, true);
        //                yield return sb.ToString();
        //                sb.Clear();
        //            }
        //        }
        //    }
        //}

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