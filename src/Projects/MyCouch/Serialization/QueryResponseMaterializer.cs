using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EnsureThat;
using MyCouch.Responses;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    /// <summary>
    /// Traverses JSON and populates members of passed <see cref="QueryResponse{T}"/> response.
    /// </summary>
    public class QueryResponseMaterializer
    {
        protected readonly SerializationConfiguration Configuration;
        protected readonly JsonSerializer InternalSerializer;
        protected readonly JsonResponseMapper JsonMapper;

        public QueryResponseMaterializer(SerializationConfiguration configuration)
        {
            Ensure.That(configuration, "configuration").IsNotNull();

            Configuration = configuration;
            InternalSerializer = JsonSerializer.Create(Configuration.Settings);
            JsonMapper = new JsonResponseMapper(Configuration);
        }

        public virtual void Populate<T>(QueryResponse<T> response, Stream data) where T : class
        {
            var mappings = new JsonResponseMappings
            {
                {JsonResponseMappings.FieldNames.TotalRows, jr => OnPopulateTotalRows(jr, response)},
                {JsonResponseMappings.FieldNames.UpdateSeq, jr => OnPopulateUpdateSeq(jr, response)},
                {JsonResponseMappings.FieldNames.Offset, jr => OnPopulateOffset(jr, response)},
                {JsonResponseMappings.FieldNames.Rows, jr => OnPopulateRows(jr, response)}
            };
            JsonMapper.Map(data, mappings);
        }

        protected virtual void OnPopulateTotalRows<T>(JsonReader jr, QueryResponse<T> response) where T : class
        {
            response.TotalRows = (long) jr.Value;
        }

        protected virtual void OnPopulateUpdateSeq<T>(JsonReader jr, QueryResponse<T> response) where T : class
        {
            response.UpdateSeq = (long)jr.Value;
        }

        protected virtual void OnPopulateOffset<T>(JsonReader jr, QueryResponse<T> response) where T : class
        {
            response.OffSet = (long)jr.Value;
        }

        protected virtual void OnPopulateRows<T>(JsonReader jr, QueryResponse<T> response) where T : class
        {
            if (response is QueryResponse<string>)
                response.Rows = YieldQueryRowsOfString(jr).ToArray() as QueryResponse<T>.Row[];
            else if (response is QueryResponse<string[]>)
                response.Rows = YieldQueryRowsOfStrings(jr).ToArray() as QueryResponse<T>.Row[];
            else
                response.Rows = InternalSerializer.Deserialize<QueryResponse<T>.Row[]>(jr); //TODO: Do as with string[]
        }

        protected virtual IEnumerable<QueryResponse<string>.Row> YieldQueryRowsOfString(JsonReader jr)
        {
            return YieldQueryRows<string>(
                jr,
                (row, jw, sb) => OnPopulateStringIfNotEmpty(jr, jw, sb, s => row.Value = s),
                (row, jw, sb) => OnPopulateStringIfNotEmpty(jr, jw, sb, s => row.Doc = s));
        }

        protected virtual IEnumerable<QueryResponse<string[]>.Row> YieldQueryRowsOfStrings(JsonReader jr)
        {
            return YieldQueryRows<string[]>(
                jr,
                (row, jw, sb) => OnPopulateStringsIfNotEmpty(jr, jw, sb, strings => row.Value = strings),
                (row, jw, sb) => OnPopulateStringsIfNotEmpty(jr, jw, sb, strings => row.Doc = strings));
        }

        protected virtual void OnPopulateStringIfNotEmpty(JsonReader jr, JsonWriter jw, StringBuilder sb, Action<string> map)
        {
            jw.WriteToken(jr, true);

            if(sb.Length > 0) map(sb.ToString());
        }

        protected virtual void OnPopulateStringsIfNotEmpty(JsonReader jr, JsonWriter jw, StringBuilder sb, Action<string[]> map)
        {
            var strings = ReadStrings(jr, jw, sb);

            if (strings.Any()) map(strings.ToArray());
        }

        protected virtual IList<string> ReadStrings(JsonReader jr, JsonWriter jw, StringBuilder sb)
        {
            var rowValues = new List<string>();

            if (jr.TokenType != JsonToken.StartArray)
                return rowValues;

            var valueStartDepth = jr.Depth;

            while (jr.Read() && !(jr.TokenType == JsonToken.EndArray && jr.Depth == valueStartDepth))
            {
                jw.WriteToken(jr, true);
                rowValues.Add(sb.ToString());
                sb.Clear();
            }

            return rowValues;
        }

        //TODO: Give some love and split it up. Remove the Ifs
        protected virtual IEnumerable<QueryResponse<T>.Row> YieldQueryRows<T>(
            JsonReader jr,
            Action<QueryResponse<T>.Row, JsonTextWriter, StringBuilder> onVisitValue,
            Action<QueryResponse<T>.Row, JsonTextWriter, StringBuilder> onVisitDoc) where T : class
        {
            if (jr.TokenType != JsonToken.StartArray)
                yield break;

            var row = new QueryResponse<T>.Row();
            var startDepth = jr.Depth;
            var sb = new StringBuilder();
            var hasMappedId = false;
            var hasMappedKey = false;
            var hasMappedValue = false;
            var hasMappedDoc = false;
            var shouldTryAndMapIncludedDoc = onVisitDoc != null;
            Action reset = () =>
            {
                hasMappedId = hasMappedKey = hasMappedValue = hasMappedDoc = false;
                row = new QueryResponse<T>.Row();
            };
            Func<bool> hasMappedSomething = () => hasMappedId || hasMappedKey || hasMappedValue || hasMappedDoc;

            using (var sw = new StringWriter(sb))
            {
                using (var jw = Configuration.ApplyConfigToWriter(new MaterializerJsonWriter(sw)))
                {
                    while (jr.Read() && !(jr.TokenType == JsonToken.EndArray && jr.Depth == startDepth))
                    {
                        if (jr.TokenType == JsonToken.EndObject)
                        {
                            if (hasMappedSomething())
                            {
                                yield return row;
                                reset();
                            }
                            continue;
                        }

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
                        else if (shouldTryAndMapIncludedDoc && propName == "doc")
                        {
                            if (!jr.Read())
                                break;

                            onVisitDoc(row, jw, sb);
                            sb.Clear();
                            hasMappedDoc = true;
                        }
                        else
                            continue;

                        if (hasMappedId && hasMappedKey && hasMappedValue)
                        {
                            if(shouldTryAndMapIncludedDoc && !hasMappedDoc)
                                continue;

                            yield return row;
                            reset();
                        }
                    }

                    if (hasMappedSomething())
                        yield return row;
                }
            }
        }

        /// <summary>
        /// When deserializing Query responses, data that is NULL should be empty
        /// instead of having a text stating null.
        /// </summary>
        protected class MaterializerJsonWriter : JsonTextWriter
        {
            public MaterializerJsonWriter(TextWriter textWriter) : base(textWriter) { }

            public override void WriteNull()
            {
                base.WriteRaw(string.Empty);
            }
        }
    }
}