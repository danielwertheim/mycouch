using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests.Factories;

namespace MyCouch.Cloudant.Requests.Factories
{
    public class SearchIndexHttpRequestFactory : HttpRequestFactoryBase
    {
        public SearchIndexHttpRequestFactory(IConnection connection) : base(connection) { }

        public virtual HttpRequest Create(SearchIndexRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            return CreateFor<SearchIndexRequest>(HttpMethod.Get, GenerateRequestUrl(request));
        }

        protected virtual string GenerateRequestUrl(SearchIndexRequest request)
        {
            return string.Format("{0}/_design/{1}/_search/{2}{3}",
                Connection.Address,
                request.IndexIdentity.DesignDocument,
                request.IndexIdentity.Name,
                GenerateQueryString(request));
        }

        protected virtual string GenerateQueryString(SearchIndexRequest request)
        {
            var p = GenerateQueryStringParams(request);

            return string.IsNullOrEmpty(p) ? string.Empty : string.Concat("?", p);
        }

        /// <summary>
        /// Generates <see cref="SearchIndexRequest"/> configured values as querystring params.
        /// </summary>
        /// <returns></returns>
        protected virtual string GenerateQueryStringParams(SearchIndexRequest request)
        {
            return string.Join("&", ConvertRequestToJsonCompatibleKeyValues(request)
                .Select(kv => string.Format("{0}={1}", kv.Key, Uri.EscapeDataString(kv.Value))));
        }

        /// <summary>
        /// Returns all configured options of <see cref="SearchIndexRequest"/> as key values.
        /// The values are formatted to JSON-compatible strings.
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary<string, string> ConvertRequestToJsonCompatibleKeyValues(SearchIndexRequest request)
        {
            var kvs = new Dictionary<string, string>();

            if (HasValue(request.Expression))
                kvs.Add(KeyNames.Expression, request.Expression);

            if (HasValue(request.Bookmark))
                kvs.Add(KeyNames.Bookmark, request.Bookmark);

            if (HasValue(request.Stale))
                kvs.Add(KeyNames.Stale, request.Stale.ToString());

            if (request.IncludeDocs.HasValue)
                kvs.Add(KeyNames.IncludeDocs, request.IncludeDocs.Value.ToString().ToLower());

            if (request.Descending.HasValue)
                kvs.Add(KeyNames.Descending, request.Descending.Value.ToString().ToLower());

            if (request.Limit.HasValue)
                kvs.Add(KeyNames.Limit, request.Limit.Value.ToString(MyCouchRuntime.NumberFormat));

            return kvs;
        }

        /// <summary>
        /// Contains the string representation (Key) of
        /// individual options for <see cref="SearchIndexRequest"/>.
        /// </summary>
        protected static class KeyNames
        {
            public const string Expression = "q";
            public const string Bookmark = "bookmark";
            public const string Stale = "stale";
            public const string IncludeDocs = "include_docs";
            public const string Descending = "descending";
            public const string Limit = "limit";
        }
    }
}