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
    public class DefaultResponseMaterializer : IResponseMaterializer
    {
        protected readonly SerializationConfiguration Configuration;
        protected readonly JsonSerializer InternalSerializer;

        public DefaultResponseMaterializer(SerializationConfiguration configuration)
        {
            Ensure.That(configuration, "configuration").IsNotNull();

            Configuration = configuration;
            InternalSerializer = JsonSerializer.Create(Configuration.Settings);
        }

        public virtual void PopulateFailedResponse<T>(T response, Stream data) where T : IResponse
        {
            var mappings = new Dictionary<string, Action<JsonTextReader>>
            {
                {"error", jr => response.Error = jr.Value.ToString()},
                {"reason", jr => response.Reason = jr.Value.ToString()}
            };
            Map(data, mappings);
        }

        public virtual void PopulateBulkResponse(BulkResponse response, Stream data)
        {
            using (var sr = new StreamReader(data))
            {
                using (var jr = Configuration.ReaderFactory(typeof(BulkResponse.Row[]), sr))
                {
                    response.Rows = InternalSerializer.Deserialize<BulkResponse.Row[]>(jr);
                }
            }
        }

        public virtual void PopulateDocumentHeaderResponse(DocumentHeaderResponse response, Stream data)
        {
            var mappings = new Dictionary<string, Action<JsonTextReader>>
            {
                {"id", jr => response.Id = jr.Value.ToString()},
                {"rev", jr => response.Rev = jr.Value.ToString()}
            };
            Map(data, mappings);
        }

        public virtual void PopulateViewQueryResponse<T>(ViewQueryResponse<T> response, Stream data) where T : class
        {
            var mappings = new Dictionary<string, Action<JsonTextReader>>
            {
                {"total_rows", jr => response.TotalRows = (long)jr.Value},
                {"update_seq", jr => response.UpdateSeq = (long)jr.Value},
                {"offset", jr => response.OffSet = (long)jr.Value},
                {"rows", jr =>
                {
                    if (response is ViewQueryResponse<string>)
                        response.Rows = YieldViewQueryRowsOfString(jr).ToArray() as ViewQueryResponse<T>.Row[];
                    else if (response is ViewQueryResponse<string[]>)
                        response.Rows = YieldViewQueryRowsOfStrings(jr).ToArray() as ViewQueryResponse<T>.Row[];
                    else
                        response.Rows = InternalSerializer.Deserialize<ViewQueryResponse<T>.Row[]>(jr); //TODO: Do as with string[]
                }},
            };
            Map(data, mappings);
        }

        protected virtual void Map(Stream data, IDictionary<string, Action<JsonTextReader>> mappings)
        {
            var numOfHandlersProcessed = 0;

            using (var sr = new StreamReader(data))
            {
                using (var jr = Configuration.ApplyConfigToReader(new JsonTextReader(sr)))
                {
                    while (jr.Read())
                    {
                        if(numOfHandlersProcessed == mappings.Count)
                            break;

                        if (jr.TokenType != JsonToken.PropertyName)
                            continue;

                        var propName = jr.Value.ToString();
                        if (!mappings.ContainsKey(propName))
                            continue;

                        if (!jr.Read())
                            break;

                        mappings[propName](jr);

                        numOfHandlersProcessed++;
                    }
                }
            }
        }

        protected virtual IEnumerable<ViewQueryResponse<string>.Row> YieldViewQueryRowsOfString(JsonReader jr)
        {
            return YieldViewQueryRows<string>(
                jr,
                (row, jw, sb) =>
                {
                    jw.WriteToken(jr, true);
                    row.Value = sb.Length > 0 ? sb.ToString() : null;
                },
                (row, jw, sb) =>
                {
                    jw.WriteToken(jr, true);
                    row.Doc = sb.Length > 0 ? sb.ToString() : null;
                });
        }

        protected virtual IEnumerable<ViewQueryResponse<string[]>.Row> YieldViewQueryRowsOfStrings(JsonReader jr)
        {
            var rowValues = new List<string>();

            return YieldViewQueryRows<string[]>(
                jr,
                (row, jw, sb) =>
                {
                    if (jr.TokenType != JsonToken.StartArray)
                        return;

                    var valueStartDepth = jr.Depth;

                    while (jr.Read() && !(jr.TokenType == JsonToken.EndArray && jr.Depth == valueStartDepth))
                    {
                        jw.WriteToken(jr, true);
                        rowValues.Add(sb.ToString());
                        sb.Clear();
                    }

                    row.Value = rowValues.ToArray();
                    rowValues.Clear();
                },
                (row, jw, sb) =>
                {
                    if (jr.TokenType != JsonToken.StartArray)
                        return;

                    var valueStartDepth = jr.Depth;

                    while (jr.Read() && !(jr.TokenType == JsonToken.EndArray && jr.Depth == valueStartDepth))
                    {
                        jw.WriteToken(jr, true);
                        rowValues.Add(sb.ToString());
                        sb.Clear();
                    }

                    row.Doc = rowValues.ToArray();
                    rowValues.Clear();
                });
        }

        protected virtual IEnumerable<ViewQueryResponse<T>.Row> YieldViewQueryRows<T>(
            JsonReader jr,
            Action<ViewQueryResponse<T>.Row, JsonTextWriter, StringBuilder> onVisitValue,
            Action<ViewQueryResponse<T>.Row, JsonTextWriter, StringBuilder> onVisitDoc = null) where T : class
        {
            if (jr.TokenType != JsonToken.StartArray)
                yield break;

            var row = new ViewQueryResponse<T>.Row();
            var startDepth = jr.Depth;
            var sb = new StringBuilder();
            var hasMappedId = false;
            var hasMappedKey = false;
            var hasMappedValue = false;
            var hasMappedDoc = false;
            var shouldTryAndMapDoc = onVisitDoc != null; //TODO: I want info about the query here so that I know if include_docs = true
            Action reset = () =>
            {
                hasMappedId = hasMappedKey = hasMappedValue = hasMappedDoc = false;
                row = new ViewQueryResponse<T>.Row();
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
                        else if (shouldTryAndMapDoc && propName == "doc")
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
                            if(shouldTryAndMapDoc && !hasMappedDoc)
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