using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnsureThat;
using MyCouch.Responses;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    /// <summary>
    /// Traverses and deserializes JSON-arrays, which should represent Rows.
    /// For use with <see cref="QueryResponse{T}.Rows"/>.
    /// </summary>
    public class QueryResponseRowsDeserializer
    {
        protected readonly JsonSerializer InternalSerializer;
        protected readonly JsonArrayItemVisitor JsonRowArrayVisitor;

        public QueryResponseRowsDeserializer(SerializationConfiguration configuration)
        {
            Ensure.That(configuration, "configuration").IsNotNull();

            InternalSerializer = JsonSerializer.Create(configuration.Settings);
            JsonRowArrayVisitor = new JsonArrayItemVisitor(configuration);
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
            Ensure.That(jr.TokenType == JsonToken.StartArray, "jr").WithExtraMessageOf(
                () => ExceptionStrings.QueryResponseRowsDeserializerNeedsJsonArray);

            var type = typeof(T);

            if (type == typeof(string))
                return DeserializeRowsOfString(jr) as IEnumerable<QueryResponse<T>.Row>;
            if (type == typeof(string[]))
                return DeserializeRowsOfStrings(jr) as IEnumerable<QueryResponse<T>.Row>;

            return InternalSerializer.Deserialize<IEnumerable<QueryResponse<T>.Row>>(jr);
        }

        protected virtual IEnumerable<QueryResponse<string>.Row> DeserializeRowsOfString(JsonReader jsonReader)
        {
            return YieldQueryRows<string>(
                jsonReader,
                (row, jr, jw, sb) => ConsumeStringIfNotEmpty(jr, jw, sb, s => row.Value = s),
                (row, jr, jw, sb) => ConsumeStringIfNotEmpty(jr, jw, sb, s => row.Doc = s));
        }

        protected virtual IEnumerable<QueryResponse<string[]>.Row> DeserializeRowsOfStrings(JsonReader jsonReader)
        {
            return YieldQueryRows<string[]>(
                jsonReader,
                (row, jr, jw, sb) => ConsumeStringsIfNotEmpty(jr, jw, sb, strings => row.Value = strings),
                (row, jr, jw, sb) => ConsumeStringsIfNotEmpty(jr, jw, sb, strings => row.Doc = strings));
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

        protected virtual IEnumerable<QueryResponse<T>.Row> YieldQueryRows<T>(
            JsonReader jsonReader, 
            JsonArrayItemVisitor.OnVisitMember<QueryResponse<T>.Row> onVisitValueMember,
            JsonArrayItemVisitor.OnVisitMember<QueryResponse<T>.Row> onVisitDocMember) where T : class
        {
            var memberHandlers = new Dictionary<string, JsonArrayItemVisitor.OnVisitMember<QueryResponse<T>.Row>>
            {
                {
                    "id", (item, jr, jw, sb) =>
                    {
                        if (!jr.Read())
                            return;

                        item.Id = jr.Value.ToString();
                    }
                },
                {
                    "key", (item, jr, jw, sb) =>
                    {
                        if (!jr.Read())
                            return;

                        item.Key = jr.Value.ToString();
                    }
                },
                {
                    "value", (item, jr, jw, sb) =>
                    {
                        if (!jr.Read())
                            return;

                        onVisitValueMember(item, jr, jw, sb);
                    }
                },
                {
                    "doc", (item, jr, jw, sb) =>
                    {
                        if (!jr.Read())
                            return;

                        onVisitDocMember(item, jr, jw, sb);
                    }
                }
            };

            return JsonRowArrayVisitor.Visit(
                jsonReader,
                () => new QueryResponse<T>.Row(),
                i => i,
                memberHandlers);
        }
    }
}