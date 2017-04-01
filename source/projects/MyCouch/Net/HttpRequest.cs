using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using EnsureThat;

namespace MyCouch.Net
{
    public class HttpRequest
    {
        public HttpMethod Method { get; private set; }
        public string RelativeUrl { get; private set; }
        public IDictionary<string, string> Headers { get; }
        public HttpContent Content { get; private set; }

        public HttpRequest(HttpMethod method) : this(method, "/") { }

        public HttpRequest(HttpMethod method, string relativeUrl)
        {
            Ensure.That(relativeUrl, "relativeUrl").IsNotNullOrWhiteSpace();

            RelativeUrl = relativeUrl;
            Method = method;
            Headers = new Dictionary<string, string> { { HttpHeaders.Accept, HttpContentTypes.Json } };
        }

        public virtual HttpRequest SetAcceptHeader(params string[] accepts)
        {
            Headers[HttpHeaders.Accept] = string.Join(",", accepts);

            return this;
        }

        public virtual HttpRequest SetIfMatchHeader(string rev)
        {
            if (!string.IsNullOrWhiteSpace(rev))
                Headers.Add(HttpHeaders.IfMatch, rev);

            return this;
        }

        public virtual HttpRequest SetContent(byte[] content, string contentType)
        {
            if (content != null && content.Length > 0)
                Content = new BytesContent(content, contentType);

            return this;
        }

        public virtual HttpRequest SetJsonContent(string content = null)
        {
            Content = string.IsNullOrWhiteSpace(content)
                ? new JsonContent()
                : new JsonContent(content);

            return this;
        }

        public virtual HttpRequest SetRequestTypeHeader(Type requestType)
        {
            Headers.Add(HttpHeaders.RequestType, GetRequestTypeName(requestType));
            var typeInfo = requestType.GetTypeInfo();
            if (typeInfo.IsGenericType)
                Headers.Add(HttpHeaders.RequestEntityType, GetRequestEntityTypeName(requestType));
            return this;
        }

        protected virtual string GetRequestTypeName(Type requestType)
        {
            var typeInfo = requestType.GetTypeInfo();
            return typeInfo.IsGenericType
                ? requestType.Name.Substring(0, requestType.Name.IndexOf('`'))
                : requestType.Name;
        }

        protected virtual string GetRequestEntityTypeName(Type requestType)
        {
            return requestType.GenericTypeArguments[0].Name;
        }

        public virtual void RemoveRequestTypeHeader()
        {
            if (Headers.ContainsKey(HttpHeaders.RequestType))
                Headers.Remove(HttpHeaders.RequestType);

            if (Headers.ContainsKey(HttpHeaders.RequestEntityType))
                Headers.Remove(HttpHeaders.RequestEntityType);
        }
    }
}