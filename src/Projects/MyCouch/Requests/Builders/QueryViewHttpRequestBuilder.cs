using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Builders
{
    public class QueryViewHttpRequestBuilder :
        RequestBuilderBase,
        IHttpRequestBuilder<QueryViewRequest>
    {
        protected readonly IConnection Connection;

        public QueryViewHttpRequestBuilder(IConnection connection)
        {
            Ensure.That(connection, "connection").IsNotNull();

            Connection = connection;
        }

        public virtual HttpRequest Create(QueryViewRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            return request.HasKeys
                ? new HttpRequest(HttpMethod.Post, GenerateRequestUrl(request)).SetContent(GetKeysAsJsonObject(request))
                : new HttpRequest(HttpMethod.Get, GenerateRequestUrl(request));
        }

        protected virtual string GenerateRequestUrl(QueryViewRequest query)
        {
            if (query is QuerySystemViewRequest)
            {
                return string.Format("{0}/{1}?{2}",
                    Connection.Address,
                    query.View.Name,
                    GenerateQueryStringParams(query));
            }

            return string.Format("{0}/_design/{1}/_view/{2}?{3}",
                Connection.Address,
                query.View.DesignDocument,
                query.View.Name,
                GenerateQueryStringParams(query));
        }

        /// <summary>
        /// Returns <see cref="QueryViewRequest.Keys"/> as compatible JSON document for use e.g.
        /// with POST of keys against views.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetKeysAsJsonObject(QueryViewRequest query)
        {
            if (!query.HasKeys)
                return "{}";

            return string.Format("{{\"keys\":[{0}]}}",
                string.Join(",", query.Keys.Select(k => FormatValue(k))));
        }

        /// <summary>
        /// Generates <see cref="QueryViewRequest"/> configured values as querystring params.
        /// </summary>
        /// <remarks><see cref="QueryViewRequest.Keys"/> are not included in this string.</remarks>
        /// <returns></returns>
        protected virtual string GenerateQueryStringParams(QueryViewRequest query)
        {
            return string.Join("&", ToJsonKeyValues(query)
                .Where(kv => kv.Key != KeyNames.Keys)
                .Select(kv => string.Format("{0}={1}", kv.Key, Uri.EscapeDataString(kv.Value))));
        }

        /// <summary>
        /// Returns all configured options of <see cref="QueryViewRequest.Options"/> as key values.
        /// The values are formatted to JSON-compatible strings.
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary<string, string> ToJsonKeyValues(QueryViewRequest query)
        {
            var kvs = new Dictionary<string, string>();

            if (query.IncludeDocs.HasValue)
                kvs.Add(KeyNames.IncludeDocs, query.IncludeDocs.Value.ToString().ToLower());

            if (query.Descending.HasValue)
                kvs.Add(KeyNames.Descending, query.Descending.Value.ToString().ToLower());

            if (query.Reduce.HasValue)
                kvs.Add(KeyNames.Reduce, query.Reduce.Value.ToString().ToLower());

            if (query.InclusiveEnd.HasValue)
                kvs.Add(KeyNames.InclusiveEnd, query.InclusiveEnd.Value.ToString().ToLower());

            if (query.UpdateSeq.HasValue)
                kvs.Add(KeyNames.UpdateSeq, query.UpdateSeq.Value.ToString().ToLower());

            if (query.Group.HasValue)
                kvs.Add(KeyNames.Group, query.Group.Value.ToString().ToLower());

            if (query.GroupLevel.HasValue)
                kvs.Add(KeyNames.GroupLevel, query.GroupLevel.Value.ToString(MyCouchRuntime.NumberFormat));

            if (HasValue(query.Stale))
                kvs.Add(KeyNames.Stale, FormatValue(query.Stale));

            if (HasValue(query.Key))
                kvs.Add(KeyNames.Key, FormatValue(query.Key));

            if (HasValue(query.Keys))
                kvs.Add(KeyNames.Keys, FormatValue(query.Keys));

            if (HasValue(query.StartKey))
                kvs.Add(KeyNames.StartKey, FormatValue(query.StartKey));

            if (HasValue(query.StartKeyDocId))
                kvs.Add(KeyNames.StartKeyDocId, FormatValue(query.StartKeyDocId));

            if (HasValue(query.EndKey))
                kvs.Add(KeyNames.EndKey, FormatValue(query.EndKey));

            if (HasValue(query.EndKeyDocId))
                kvs.Add(KeyNames.EndKeyDocId, FormatValue(query.EndKeyDocId));

            if (query.Limit.HasValue)
                kvs.Add(KeyNames.Limit, query.Limit.Value.ToString(MyCouchRuntime.NumberFormat));

            if (query.Skip.HasValue)
                kvs.Add(KeyNames.Skip, query.Skip.Value.ToString(MyCouchRuntime.NumberFormat));

            return kvs;
        }

        /// <summary>
        /// Contains the string representation (Key) of
        /// individual options for <see cref="QueryViewRequest"/>.
        /// </summary>
        protected static class KeyNames
        {
            public const string IncludeDocs = "include_docs";
            public const string Descending = "descending";
            public const string Reduce = "reduce";
            public const string InclusiveEnd = "inclusive_end";
            public const string UpdateSeq = "update_seq";
            public const string Group = "group";
            public const string GroupLevel = "group_level";
            public const string Stale = "stale";
            public const string Key = "key";
            public const string Keys = "keys";
            public const string StartKey = "startkey";
            public const string StartKeyDocId = "startkey_docid";
            public const string EndKey = "endkey";
            public const string EndKeyDocId = "endkey_docid";
            public const string Limit = "limit";
            public const string Skip = "skip";
        }
    }
}