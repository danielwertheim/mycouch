using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Serialization;
using System;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;

namespace MyCouch.HttpRequestFactories
{
    public class ShowHttpRequestFactory
    {
        protected ISerializer Serializer { get; private set; }

        public ShowHttpRequestFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            Serializer = serializer;
        }

        public virtual HttpRequest Create(ShowRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = new HttpRequest(HttpMethod.Get, GenerateRelativeUrl(request));

            httpRequest.SetRequestTypeHeader(request.GetType());

            return httpRequest;
        }

        protected virtual string GenerateRelativeUrl(ShowRequest request)
        {
            if(!string.IsNullOrWhiteSpace(request.Id))
                return string.Format("/_design/{0}/_show/{1}/{2}{3}",
                    new UrlSegment(request.ShowIdentity.DesignDocument),
                    new UrlSegment(request.ShowIdentity.Name),
                    new UrlSegment(request.Id),
                    GenerateRequestUrlQueryString(request));
            else
                return string.Format("/_design/{0}/_show/{1}{2}",
                    new UrlSegment(request.ShowIdentity.DesignDocument),
                    new UrlSegment(request.ShowIdentity.Name),
                    GenerateRequestUrlQueryString(request));
        }

        protected virtual string GenerateRequestUrlQueryString(ShowRequest request)
        {
            var p = GenerateCustomQueryStringParams(request);

            return string.IsNullOrWhiteSpace(p) ? string.Empty : string.Concat("?", p);
        }

        protected virtual string GenerateCustomQueryStringParams(ShowRequest request)
        {
            var customQueryParameters = GenerateJsonCompatibleKeyValues(request);
            return customQueryParameters.Any() ?
                string.Join("&", GenerateJsonCompatibleKeyValues(request)
                .Select(kv => string.Format("{0}={1}", kv.Key, UrlParam.Encode(kv.Value)))) :
                string.Empty;
        }

        protected virtual IDictionary<string, string> GenerateJsonCompatibleKeyValues(ShowRequest request)
        {
            var kvs = new Dictionary<string, string>();

            if (request.HasCustomQueryParameters)
                foreach (var param in request.CustomQueryParameters)
                    kvs.Add(UrlParam.Encode(param.Key), Serializer.ToJson(param.Value));

            return kvs;
        }
    }
}
