using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace MyCouch.HttpRequestFactories
{
    public class QueryShowHttpRequestFactory
    {
        protected ISerializer Serializer { get; private set; }

        public QueryShowHttpRequestFactory(ISerializer serializer)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            Serializer = serializer;
        }

        public virtual HttpRequest Create(QueryShowRequest request)
        {
            Ensure.Any.IsNotNull(request, nameof(request));

            var httpRequest = new HttpRequest(HttpMethod.Get, GenerateRelativeUrl(request));

            httpRequest.SetRequestTypeHeader(request.GetType());

            if (request.HasAccepts)
                httpRequest.SetAcceptHeader(request.Accepts);
            else
                httpRequest.SetAcceptHeader(HttpContentTypes.Json, HttpContentTypes.Html);

            return httpRequest;
        }

        protected virtual string GenerateRelativeUrl(QueryShowRequest request)
        {
            if(!string.IsNullOrWhiteSpace(request.DocId))
                return string.Format("/_design/{0}/_show/{1}/{2}{3}",
                    new UrlSegment(request.ShowIdentity.DesignDocument),
                    new UrlSegment(request.ShowIdentity.Name),
                    new UrlSegment(request.DocId),
                    GenerateRequestUrlQueryString(request));
            else
                return string.Format("/_design/{0}/_show/{1}{2}",
                    new UrlSegment(request.ShowIdentity.DesignDocument),
                    new UrlSegment(request.ShowIdentity.Name),
                    GenerateRequestUrlQueryString(request));
        }

        protected virtual string GenerateRequestUrlQueryString(QueryShowRequest request)
        {
            var p = GenerateQueryStringParams(request);

            return string.IsNullOrEmpty(p) ? string.Empty : string.Concat("?", p);
        }

        protected virtual string GenerateQueryStringParams(QueryShowRequest request)
        {
            return string.Join("&", GenerateJsonCompatibleKeyValues(request)
                .Select(kv => string.Format("{0}={1}", kv.Key, UrlParam.Encode(kv.Value))));
        }

        protected virtual IDictionary<string, string> GenerateJsonCompatibleKeyValues(QueryShowRequest request)
        {
            var kvs = new Dictionary<string, string>();

            if (request.HasCustomQueryParameters)
                foreach (var param in request.CustomQueryParameters)
                    kvs.Add(param.Key, param.Value.ToStringExtended());

            return kvs;
        }
    }
}
