using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MyCouch.Schemes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyCouch.Serialization
{
    public class MyCouchSerializer : ISerializer
    {
        protected readonly IEntityAccessor EntityAccessor;
        protected readonly JsonSerializer VanillaSerializer;
        protected readonly JsonSerializer EntitySerializer;

        public MyCouchSerializer(IEntityAccessor entityAccessor)
        {
            EntityAccessor = entityAccessor;
            VanillaSerializer = JsonSerializer.Create(CreateSettings(new DefaultContractResolver(true)));
            EntitySerializer = JsonSerializer.Create(CreateSettings(new SerializationContractResolver(EntityAccessor)));
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
                VanillaSerializer.Serialize(textWriter, item);
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
                    EntitySerializer.Serialize(jsonWriter, entity);
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
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return VanillaSerializer.Deserialize<T>(jsonReader);
                }
            }
        }

        public virtual T DeserializeEntity<T>(Stream data) where T : class
        {
            using (var reader = new StreamReader(data, Encoding.UTF8))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return EntitySerializer.Deserialize<T>(jsonReader);
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
                        if (jr.TokenType != JsonToken.PropertyName) 
                            continue;
                        
                        var propName = jr.Value.ToString();
                        if (propName == "id")
                        {
                            if (!jr.Read())
                                break;
                            response.Id = jr.Value.ToString();
                        }
                        else if (propName == "rev")
                        {
                            if (!jr.Read())
                                break;
                            response.Rev = jr.Value.ToString();
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

                        if (propName == "update_seq")
                        {
                            if (!jr.Read())
                                break;
                            response.UpdateSeq = (long)jr.Value;
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
                                response.Rows = YieldViewQueryRowsOfString(jr).ToArray() as ViewQueryResponse<T>.Row[];
                            else if (response is ViewQueryResponse<string[]>)
                                response.Rows = YieldViewQueryRowsOfStrings(jr).ToArray() as ViewQueryResponse<T>.Row[];
                            else if (response is ViewQueryResponse<object>)
                                response.Rows = VanillaSerializer.Deserialize<ViewQueryResponse<T>.Row[]>(jr);
                            else
                                response.Rows = EntitySerializer.Deserialize<ViewQueryResponse<T>.Row[]>(jr);
                        }
                    }
                }
            }
        }

        protected IEnumerable<ViewQueryResponse<string>.Row> YieldViewQueryRowsOfString(JsonReader jr)
        {
            return YieldViewQueryRows<string>(jr, (row, jw, sb) =>
            {
                jw.WriteToken(jr, true);
                row.Value = sb.ToString();
            });
        }

        protected IEnumerable<ViewQueryResponse<string[]>.Row> YieldViewQueryRowsOfStrings(JsonReader jr)
        {
            var rowValues = new List<string>();

            return YieldViewQueryRows<string[]>(jr, (row, jw, sb) =>
            {
                var valueStartDepth = jr.Depth;

                while (jr.Read() && !(jr.TokenType == JsonToken.EndArray && jr.Depth == valueStartDepth))
                {
                    jw.WriteToken(jr, true);
                    rowValues.Add(sb.ToString());
                    sb.Clear();
                }

                row.Value = rowValues.ToArray();
                rowValues.Clear();
            });
        }

        protected IEnumerable<ViewQueryResponse<T>.Row> YieldViewQueryRows<T>(JsonReader jr, Action<ViewQueryResponse<T>.Row, JsonWriter, StringBuilder> onVisitValue) where T : class 
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

                            onVisitValue(row, jw, sb);
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

        public virtual void PopulateFailedResponse<T>(T response, Stream data) where T : Response
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
                        if (propName == "error")
                        {
                            if (!jr.Read())
                                break;
                            response.Error = jr.Value.ToString();
                        }
                        else if (propName == "reason")
                        {
                            if (!jr.Read())
                                break;
                            response.Reason = jr.Value.ToString();
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