using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnsureThat;
using MyCouch.Responses;
using MyCouch.Responses.Meta;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    /// <summary>
    /// Traverses and deserializes JSON-arrays, which should represent Rows.
    /// For use with e.g. <see cref="QueryResponse{T}.Rows"/>.
    /// </summary>
    public class ViewQueryResponseRowsDeserializer : IQueryResponseRowsDeserializer
    {
        protected readonly JsonSerializer InternalSerializer;
        protected readonly JsonArrayItemVisitor JsonRowArrayVisitor;

        public ViewQueryResponseRowsDeserializer(SerializationConfiguration configuration)
        {
            Ensure.That(configuration, "configuration").IsNotNull();

            InternalSerializer = JsonSerializer.Create(configuration.Settings);
            JsonRowArrayVisitor = new JsonArrayItemVisitor(configuration);
        }

        /// <summary>
        /// Takes a <see cref="JsonReader"/>, which should point to a node being
        /// an array. Traverses the tree and yields <see cref="ViewQueryResponse{T}.Row"/>
        /// from it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jr"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> Deserialize<T>(JsonReader jr) where T : QueryResponseRow
        {
            Ensure.That(jr.TokenType == JsonToken.StartArray, "jr").WithExtraMessageOf(
                () => ExceptionStrings.QueryResponseRowsDeserializerNeedsJsonArray);

            var type = typeof(T);

            if (type == typeof(ViewQueryResponse<string>.Row))
                return DeserializeRowsOfString(jr) as IEnumerable<T>;
            if (type == typeof(ViewQueryResponse<string[]>.Row))
                return DeserializeRowsOfStrings(jr) as IEnumerable<T>;

            return InternalSerializer.Deserialize<IEnumerable<T>>(jr);
        }

        protected virtual IEnumerable<ViewQueryResponse<string>.Row> DeserializeRowsOfString(JsonReader jsonReader)
        {
            return YieldQueryRows<string>(
                jsonReader,
                (row, jr, jw, sb) => ConsumeStringIfNotEmpty(jr, jw, sb, s => row.Value = s),
                (row, jr, jw, sb) => ConsumeStringIfNotEmpty(jr, jw, sb, s => row.IncludedDoc = s));
        }

        protected virtual IEnumerable<ViewQueryResponse<string[]>.Row> DeserializeRowsOfStrings(JsonReader jsonReader)
        {
            return YieldQueryRows<string[]>(
                jsonReader,
                (row, jr, jw, sb) => ConsumeStringsIfNotEmpty(jr, jw, sb, strings => row.Value = strings),
                (row, jr, jw, sb) => ConsumeStringsIfNotEmpty(jr, jw, sb, strings => row.IncludedDoc = strings));
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

        protected virtual IEnumerable<ViewQueryResponse<T>.Row> YieldQueryRows<T>(
            JsonReader jsonReader,
            JsonArrayItemVisitor.OnVisitMember<ViewQueryResponse<T>.Row> onVisitValueMember,
            JsonArrayItemVisitor.OnVisitMember<ViewQueryResponse<T>.Row> onVisitDocMember)
        {
            var memberHandlers = new Dictionary<string, JsonArrayItemVisitor.OnVisitMember<ViewQueryResponse<T>.Row>>
            {
                {
                    ResponseMeta.Scheme.Queries.RowId, (item, jr, jw, sb) =>
                    {
                        if (!jr.Read())
                            return;

                        if (jr.Value != null)
                            item.Id = jr.Value.ToString();
                    }
                },
                {
                    ResponseMeta.Scheme.Queries.RowKey, (item, jr, jw, sb) =>
                    {
                        if (!jr.Read() || jr.Value == null)
                            return;

                        item.Key = jr.Value;
                    }
                },
                {
                    ResponseMeta.Scheme.Queries.RowValue, (item, jr, jw, sb) =>
                    {
                        if (!jr.Read())
                            return;

                        onVisitValueMember(item, jr, jw, sb);
                    }
                },
                {
                    ResponseMeta.Scheme.Queries.RowDoc, (item, jr, jw, sb) =>
                    {
                        if (!jr.Read())
                            return;

                        onVisitDocMember(item, jr, jw, sb);
                    }
                }
            };

            return JsonRowArrayVisitor.Visit(
                jsonReader,
                () => new ViewQueryResponse<T>.Row(),
                i => i,
                memberHandlers);
        }
    }
}