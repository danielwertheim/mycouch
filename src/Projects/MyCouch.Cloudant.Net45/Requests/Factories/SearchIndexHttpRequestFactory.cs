using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Net;
using MyCouch.Requests.Factories;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Requests.Factories
{
    public class SearchIndexHttpRequestFactory : HttpRequestFactoryBase
    {
        protected ISerializer Serializer { get; private set; }

        public SearchIndexHttpRequestFactory(IConnection connection, ISerializer serializer) : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            Serializer = serializer;
        }

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
                GenerateRequestUrlQueryString(request));
        }

        protected virtual string GenerateRequestUrlQueryString(SearchIndexRequest request)
        {
            var p = GenerateQueryStringParams(request);

            return string.IsNullOrEmpty(p) ? string.Empty : string.Concat("?", p);
        }

        protected virtual string GenerateQueryStringParams(SearchIndexRequest request)
        {
            return string.Join("&", GenerateJsonCompatibleKeyValues(request)
                .Select(kv => string.Format("{0}={1}", kv.Key, Uri.EscapeDataString(kv.Value))));
        }

        protected virtual IDictionary<string, string> GenerateJsonCompatibleKeyValues(SearchIndexRequest request)
        {
            var kvs = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(request.Expression))
                kvs.Add(KeyNames.Expression, request.Expression);

            if (request.HasSortings())
                kvs.Add(KeyNames.Sort, Serializer.ToJsonArray(request.Sort.ToArray()));

            if (!string.IsNullOrWhiteSpace(request.Bookmark))
                kvs.Add(KeyNames.Bookmark, request.Bookmark);

            if (request.Stale.HasValue)
                kvs.Add(KeyNames.Stale, request.Stale.Value.AsString());

            if (request.Limit.HasValue)
                kvs.Add(KeyNames.Limit, Serializer.ToJson(request.Limit.Value));

            if (request.IncludeDocs.HasValue)
                kvs.Add(KeyNames.IncludeDocs, Serializer.ToJson(request.IncludeDocs.Value));

            return kvs;
        }

        protected static class KeyNames
        {
            public const string Expression = "q";
            public const string Sort = "sort";
            public const string Bookmark = "bookmark";
            public const string Stale = "stale";
            public const string Limit = "limit";
            public const string IncludeDocs = "include_docs";
        }
    }
}