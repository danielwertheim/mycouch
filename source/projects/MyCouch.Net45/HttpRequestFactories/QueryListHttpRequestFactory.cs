using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Serialization;

namespace MyCouch.HttpRequestFactories
{
    public class QueryListHttpRequestFactory
    {
        protected ISerializer Serializer { get; private set; }

        public QueryListHttpRequestFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            Serializer = serializer;
        }

        public virtual HttpRequest Create(QueryListRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = new HttpRequest(HttpMethod.Get, GenerateRelativeUrl(request));

            httpRequest.SetRequestTypeHeader(request.GetType());

            return httpRequest;
        }

        protected virtual string GenerateRelativeUrl(QueryListRequest request)
        {
            return string.Format("/_design/{0}/_list/{1}/{2}{3}",
                    request.ListIdentity.DesignDocument,
                    request.ListIdentity.Name,
                    request.ViewName,
                    GenerateRequestUrlQueryString(request));
        }

        protected virtual string GenerateRequestUrlQueryString(QueryListRequest request)
        {
            var p = GenerateQueryStringParams(request);

            return string.IsNullOrWhiteSpace(p) ? string.Empty : string.Concat("?", p);
        }

        protected virtual string GenerateQueryStringParams(QueryListRequest request)
        {
            return string.Join("&", GenerateJsonCompatibleKeyValues(request)
                .Select(kv => string.Format("{0}={1}", kv.Key, UrlParam.Encode(kv.Value))));
        }

        /// <summary>
        /// Returns all configured options of <see cref="QueryListRequest"/> as key values.
        /// The values are formatted to JSON-compatible strings.
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary<string, string> GenerateJsonCompatibleKeyValues(QueryListRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var kvs = new Dictionary<string, string>();

            if (request.Key != null)
                kvs.Add(KeyNames.Key, Serializer.ToJson(request.Key));

            if(request.HasAdditionalQueryParameters)
            {
                foreach (var param in request.AdditionalQueryParameters)
                    kvs.Add(param.Key, Serializer.ToJson(param.Value));
            }

            return kvs;
        }

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
            public const string StartKey = "startkey";
            public const string StartKeyDocId = "startkey_docid";
            public const string EndKey = "endkey";
            public const string EndKeyDocId = "endkey_docid";
            public const string Limit = "limit";
            public const string Skip = "skip";
        }
    }
}