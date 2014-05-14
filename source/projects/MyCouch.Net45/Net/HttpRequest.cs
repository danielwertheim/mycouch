using System;
using System.Collections.Generic;
using System.Net.Http;
#if PCL
using System.Reflection;
using MyCouch.Extensions;
#endif
using EnsureThat;

namespace MyCouch.Net
{
#if !PCL
    [Serializable]
#endif
    public class HttpRequest
    {
        public HttpMethod Method { get; private set; }
        public string RelativeUrl { get; private set; }
        public IDictionary<string, string> Headers { get; private set; }
        public HttpContent Content { get; private set; }

        public HttpRequest(HttpMethod method, string relativeUrl)
        {
            Ensure.That(relativeUrl, "relativeUrl").IsNotNullOrWhiteSpace();

            RelativeUrl = relativeUrl;
            Method = method;
            Headers = new Dictionary<string, string> { { "Accept", HttpContentTypes.Json } };
        }

        public virtual void SetIfMatch(string rev)
        {
            if (!string.IsNullOrWhiteSpace(rev))
                Headers.Add("If-Match", rev);
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

        public virtual HttpRequest SetRequestType(Type requestType)
        {
            Headers.Add(CustomHeaders.RequestType, GetRequestTypeName(requestType));
#if !PCL
            if (requestType.IsGenericType)
                Headers.Add(CustomHeaders.RequestEntityType, GetRequestEntityTypeName(requestType));
#else
            var typeInfo = requestType.GetTypeInfo();
            if(typeInfo.IsGenericType)
                Headers.Add(CustomHeaders.RequestEntityType, GetRequestEntityTypeName(requestType));
#endif
            return this;
        }

        protected virtual string GetRequestTypeName(Type requestType)
        {
#if !PCL
            return requestType.IsGenericType
                ? requestType.Name.Substring(0, requestType.Name.IndexOf('`'))
                : requestType.Name;
#else
            var typeInfo = requestType.GetTypeInfo();
            return typeInfo.IsGenericType
                ? requestType.Name.Substring(0, requestType.Name.IndexOf('`'))
                : requestType.Name;
#endif
        }

        protected virtual string GetRequestEntityTypeName(Type requestType)
        {
#if net45
            return requestType.GenericTypeArguments[0].Name;
#endif
#if net40
            return requestType.GetGenericArguments()[0].Name;
#endif
#if PCL
            return requestType.GenericTypeArguments[0].Name;
#endif
        }

        public virtual void RemoveRequestType()
        {
            if (Headers.ContainsKey(CustomHeaders.RequestType))
                Headers.Remove(CustomHeaders.RequestType);

            if (Headers.ContainsKey(CustomHeaders.RequestEntityType))
                Headers.Remove(CustomHeaders.RequestEntityType);
        }

        public static class CustomHeaders
        {
            public const string RequestType = "mycouch-request-type";
            public const string RequestEntityType = "mycouch-entitytype";
        }
    }
}