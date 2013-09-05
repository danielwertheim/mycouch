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
    /// Traverses and deserializes JSON-arrays, which should represent Rows.
    /// For use with e.g. <see cref="QueryResponse{T}.Rows"/>.
    /// </summary>
    public class QueryResponseRowsDeserializer
    {
        protected readonly SerializationConfiguration Configuration;
        protected readonly JsonSerializer InternalSerializer;

        public QueryResponseRowsDeserializer(SerializationConfiguration configuration)
        {
            Ensure.That(configuration, "configuration").IsNotNull();

            Configuration = configuration;
            InternalSerializer = JsonSerializer.Create(Configuration.Settings);
        }

        /// <summary>
        /// Takes a <see cref="JsonReader"/>, which should point to a node being
        /// an array. Traverses the tree and yields <see cref="QueryResponse{T}.Row"/>
        /// from it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jr"></param>
        /// <returns></returns>
        public virtual IEnumerable<QueryResponse<T>.Row> Deserialize<T>(JsonReader jr) where T : class
        {
            if (jr.TokenType != JsonToken.StartArray)
                throw new MyCouchException(ExceptionStrings.QueryResponseRowsDeserializerNeedsJsonArray);

            var type = typeof(T);

            if (type == typeof(string))
                return DeserializeRowsOfString(jr) as IEnumerable<QueryResponse<T>.Row>;
            if (type == typeof(string[]))
                return DeserializeRowsOfStrings(jr) as IEnumerable<QueryResponse<T>.Row>;

            return InternalSerializer.Deserialize<IEnumerable<QueryResponse<T>.Row>>(jr); //TODO: Do as with string[]
        }

        protected virtual IEnumerable<QueryResponse<string>.Row> DeserializeRowsOfString(JsonReader jsonReader)
        {
            return YieldQueryRows<string>(
                jsonReader,
                (jr, jw, sb, row) => ConsumeStringIfNotEmpty(jr, jw, sb, s => row.Value = s),
                (jr, jw, sb, row) => ConsumeStringIfNotEmpty(jr, jw, sb, s => row.Doc = s));
        }

        protected virtual IEnumerable<QueryResponse<string[]>.Row> DeserializeRowsOfStrings(JsonReader jsonReader)
        {
            return YieldQueryRows<string[]>(
                jsonReader,
                (jr, jw, sb, row) => ConsumeStringsIfNotEmpty(jr, jw, sb, strings => row.Value = strings),
                (jr, jw, sb, row) => ConsumeStringsIfNotEmpty(jr, jw, sb, strings => row.Doc = strings));
        }

        protected virtual void ConsumeStringIfNotEmpty(JsonReader jr, JsonWriter jw, StringBuilder sb, Action<string> map)
        {
            jw.WriteToken(jr, true);

            if (sb.Length > 0) map(sb.ToString());
        }

        protected virtual void ConsumeStringsIfNotEmpty(JsonReader jr, JsonWriter jw, StringBuilder sb, Action<string[]> map)
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
            Action<JsonReader, JsonWriter, StringBuilder, QueryResponse<T>.Row> onVisitValue,
            Action<JsonReader, JsonWriter, StringBuilder, QueryResponse<T>.Row> onVisitDoc) where T : class
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

                            onVisitValue(jr, jw, sb, row);
                            sb.Clear();
                            hasMappedValue = true;
                        }
                        else if (shouldTryAndMapIncludedDoc && propName == "doc")
                        {
                            if (!jr.Read())
                                break;

                            onVisitDoc(jr, jw, sb, row);
                            sb.Clear();
                            hasMappedDoc = true;
                        }
                        else
                            continue;

                        if (hasMappedId && hasMappedKey && hasMappedValue)
                        {
                            if (shouldTryAndMapIncludedDoc && !hasMappedDoc)
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