using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Querying;

namespace MyCouch.Requests.Builders
{
    public class QueryViewRequestBuilder :
        RequestBuilderBase,
        IRequestBuilder<QueryViewRequest>
    {
        protected readonly IConnection Connection;

        public QueryViewRequestBuilder(IConnection connection)
        {
            Ensure.That(connection, "connection").IsNotNull();

            Connection = connection;
        }

        public virtual HttpRequestMessage Create(QueryViewRequest cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            return cmd.Options.HasKeys
                ? new HttpRequest(HttpMethod.Post, GenerateRequestUrl(cmd)).SetContent(GetKeysAsJsonObject(cmd))
                : new HttpRequest(HttpMethod.Get, GenerateRequestUrl(cmd));
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
        /// Returns <see cref="ViewQueryOptions.Keys"/> as compatible JSON document for use e.g.
        /// with POST of keys against views.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetKeysAsJsonObject(QueryViewRequest query)
        {
            if (!query.Options.HasKeys)
                return "{}";

            return string.Format("{{\"keys\":[{0}]}}",
                string.Join(",", query.Options.Keys.Select(k => FormatValue(k))));
        }

        /// <summary>
        /// Generates <see cref="ViewQueryOptions"/> configured values as querystring params.
        /// </summary>
        /// <remarks><see cref="ViewQueryOptions.Keys"/> are not included in this string.</remarks>
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

            if (query.Options.IncludeDocs.HasValue)
                kvs.Add(KeyNames.IncludeDocs, query.Options.IncludeDocs.Value.ToString().ToLower());

            if (query.Options.Descending.HasValue)
                kvs.Add(KeyNames.Descending, query.Options.Descending.Value.ToString().ToLower());

            if (query.Options.Reduce.HasValue)
                kvs.Add(KeyNames.Reduce, query.Options.Reduce.Value.ToString().ToLower());

            if (query.Options.InclusiveEnd.HasValue)
                kvs.Add(KeyNames.InclusiveEnd, query.Options.InclusiveEnd.Value.ToString().ToLower());

            if (query.Options.UpdateSeq.HasValue)
                kvs.Add(KeyNames.UpdateSeq, query.Options.UpdateSeq.Value.ToString().ToLower());

            if (query.Options.Group.HasValue)
                kvs.Add(KeyNames.Group, query.Options.Group.Value.ToString().ToLower());

            if (query.Options.GroupLevel.HasValue)
                kvs.Add(KeyNames.GroupLevel, query.Options.GroupLevel.Value.ToString(MyCouchRuntime.NumberFormat));

            if (HasValue(query.Options.Stale))
                kvs.Add(KeyNames.Stale, FormatValue(query.Options.Stale));

            if (HasValue(query.Options.Key))
                kvs.Add(KeyNames.Key, FormatValue(query.Options.Key));

            if (HasValue(query.Options.Keys))
                kvs.Add(KeyNames.Keys, FormatValue(query.Options.Keys));

            if (HasValue(query.Options.StartKey))
                kvs.Add(KeyNames.StartKey, FormatValue(query.Options.StartKey));

            if (HasValue(query.Options.StartKeyDocId))
                kvs.Add(KeyNames.StartKeyDocId, FormatValue(query.Options.StartKeyDocId));

            if (HasValue(query.Options.EndKey))
                kvs.Add(KeyNames.EndKey, FormatValue(query.Options.EndKey));

            if (HasValue(query.Options.EndKeyDocId))
                kvs.Add(KeyNames.EndKeyDocId, FormatValue(query.Options.EndKeyDocId));

            if (query.Options.Limit.HasValue)
                kvs.Add(KeyNames.Limit, query.Options.Limit.Value.ToString(MyCouchRuntime.NumberFormat));

            if (query.Options.Skip.HasValue)
                kvs.Add(KeyNames.Skip, query.Options.Skip.Value.ToString(MyCouchRuntime.NumberFormat));

            return kvs;
        }

        /// <summary>
        /// Contains the string representation (Key) of
        /// individual options for <see cref="ViewQueryOptions"/>.
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