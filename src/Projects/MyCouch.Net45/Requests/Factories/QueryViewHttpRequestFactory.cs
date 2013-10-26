using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class QueryViewHttpRequestFactory : HttpRequestFactoryBase
    {
        public QueryViewHttpRequestFactory(IConnection connection) : base(connection) { }

        public virtual HttpRequest Create(QueryViewRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            return request.HasKeys
                ? CreateFor<QueryViewRequest>(HttpMethod.Post, GenerateRequestUrl(request)).SetContent(GetKeysAsJsonObject(request))
                : CreateFor<QueryViewRequest>(HttpMethod.Get, GenerateRequestUrl(request));
        }

        protected virtual string GenerateRequestUrl(QueryViewRequest request)
        {
            if (request is QuerySystemViewRequest)
            {
                return string.Format("{0}/{1}{2}",
                    Connection.Address,
                    request.View.Name,
                    GenerateQueryString(request));
            }

            return string.Format("{0}/_design/{1}/_view/{2}{3}",
                Connection.Address,
                request.View.DesignDocument,
                request.View.Name,
                GenerateQueryString(request));
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

        protected virtual string GenerateQueryString(QueryViewRequest request)
        {
            var p = GenerateQueryStringParams(request);

            return string.IsNullOrEmpty(p) ? string.Empty : string.Concat("?", p);
        }

        /// <summary>
        /// Generates <see cref="QueryViewRequest"/> configured values as querystring params.
        /// </summary>
        /// <remarks><see cref="QueryViewRequest.Keys"/> are not included in this string.</remarks>
        /// <returns></returns>
        protected virtual string GenerateQueryStringParams(QueryViewRequest request)
        {
            return string.Join("&", ConvertRequestToJsonCompatibleKeyValues(request)
                .Where(kv => kv.Key != KeyNames.Keys)
                .Select(kv => string.Format("{0}={1}", kv.Key, Uri.EscapeDataString(kv.Value))));
        }

        /// <summary>
        /// Returns all configured options of <see cref="QueryViewRequest"/> as key values.
        /// The values are formatted to JSON-compatible strings.
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary<string, string> ConvertRequestToJsonCompatibleKeyValues(QueryViewRequest request)
        {
            var kvs = new Dictionary<string, string>();

            if (request.IncludeDocs.HasValue)
                kvs.Add(KeyNames.IncludeDocs, request.IncludeDocs.Value.ToString().ToLower());

            if (request.Descending.HasValue)
                kvs.Add(KeyNames.Descending, request.Descending.Value.ToString().ToLower());

            if (request.Reduce.HasValue)
                kvs.Add(KeyNames.Reduce, request.Reduce.Value.ToString().ToLower());

            if (request.InclusiveEnd.HasValue)
                kvs.Add(KeyNames.InclusiveEnd, request.InclusiveEnd.Value.ToString().ToLower());

            if (request.UpdateSeq.HasValue)
                kvs.Add(KeyNames.UpdateSeq, request.UpdateSeq.Value.ToString().ToLower());

            if (request.Group.HasValue)
                kvs.Add(KeyNames.Group, request.Group.Value.ToString().ToLower());

            if (request.GroupLevel.HasValue)
                kvs.Add(KeyNames.GroupLevel, request.GroupLevel.Value.ToString(MyCouchRuntime.NumberFormat));

            if (HasValue(request.Stale))
                kvs.Add(KeyNames.Stale, FormatValue(request.Stale));

            if (HasValue(request.Key))
                kvs.Add(KeyNames.Key, FormatValue(request.Key));

            if (HasValue(request.Keys))
                kvs.Add(KeyNames.Keys, FormatValue(request.Keys));

            if (HasValue(request.StartKey))
                kvs.Add(KeyNames.StartKey, FormatValue(request.StartKey));

            if (HasValue(request.StartKeyDocId))
                kvs.Add(KeyNames.StartKeyDocId, FormatValue(request.StartKeyDocId));

            if (HasValue(request.EndKey))
                kvs.Add(KeyNames.EndKey, FormatValue(request.EndKey));

            if (HasValue(request.EndKeyDocId))
                kvs.Add(KeyNames.EndKeyDocId, FormatValue(request.EndKeyDocId));

            if (request.Limit.HasValue)
                kvs.Add(KeyNames.Limit, request.Limit.Value.ToString(MyCouchRuntime.NumberFormat));

            if (request.Skip.HasValue)
                kvs.Add(KeyNames.Skip, request.Skip.Value.ToString(MyCouchRuntime.NumberFormat));

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