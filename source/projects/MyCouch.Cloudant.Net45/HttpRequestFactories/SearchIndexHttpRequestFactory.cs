using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using EnsureThat;
using MyCouch.Cloudant.Requests;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.HttpRequestFactories
{
    public class SearchIndexHttpRequestFactory
    {
        protected ISerializer DocumentSerializer { get; private set; }
        protected ISerializer Serializer { get; private set; }

        public SearchIndexHttpRequestFactory(ISerializer documentSerializer, ISerializer serializer)
        {
            Ensure.That(documentSerializer, "DocumentSerializer").IsNotNull();
            Ensure.That(serializer, "Serializer").IsNotNull();

            DocumentSerializer = documentSerializer;
            Serializer = serializer;
        }

        public virtual HttpRequest Create(SearchIndexRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            return new HttpRequest(HttpMethod.Get, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType());
        }

        protected virtual string GenerateRelativeUrl(SearchIndexRequest request)
        {
            return string.Format("/_design/{0}/_search/{1}{2}",
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
                kvs.Add(KeyNames.Sort, DocumentSerializer.ToJsonArray(request.Sort.ToArray()));

            if (!string.IsNullOrWhiteSpace(request.Bookmark))
                kvs.Add(KeyNames.Bookmark, request.Bookmark);

            if (request.Stale.HasValue)
                kvs.Add(KeyNames.Stale, request.Stale.Value.AsString());

            if (request.Limit.HasValue)
                kvs.Add(KeyNames.Limit, DocumentSerializer.ToJson(request.Limit.Value));

            if (request.IncludeDocs.HasValue)
                kvs.Add(KeyNames.IncludeDocs, DocumentSerializer.ToJson(request.IncludeDocs.Value));
            
            if (request.Ranges != null)
                kvs.Add(KeyNames.Ranges, Serializer.Serialize(request.Ranges));

            if (request.HasCounts())
                kvs.Add(KeyNames.Counts, DocumentSerializer.ToJsonArray(request.Counts.ToArray()));

            if (!string.IsNullOrWhiteSpace(request.GroupField))
                kvs.Add(KeyNames.GroupField, request.GroupField);

            if (request.GroupLimit.HasValue)
                kvs.Add(KeyNames.GroupLimit, DocumentSerializer.ToJson(request.GroupLimit.Value));

            if (request.HasGroupSortings())
                kvs.Add(KeyNames.GroupSort, DocumentSerializer.ToJsonArray(request.GroupSort.ToArray()));

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
            public const string Ranges = "ranges";
            public const string Counts = "counts";
            public const string GroupField = "group_field";
            public const string GroupLimit = "group_limit";
            public const string GroupSort = "group_sort";
        }
    }
}