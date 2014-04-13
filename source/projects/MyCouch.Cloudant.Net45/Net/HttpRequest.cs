using System;
using System.Net.Http;
using System.Net.Http.Headers;
#if PCL
using System.Reflection;
using MyCouch.Extensions;
#endif

namespace MyCouch.Net
{
#if !PCL
    [Serializable]
#endif
    public class HttpRequest : HttpRequestMessage
    {
        public static class CustomHeaders
        {
            public const string RequestType = "mycouch-request-type";
            public const string RequestEntityType = "mycouch-entitytype";
        }

        public HttpRequest(HttpMethod method, string url) : base(method, new Uri(url))
        {
            Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpContentTypes.Json));
        }

        public virtual void SetIfMatch(string rev)
        {
            if(!string.IsNullOrWhiteSpace(rev))
                Headers.TryAddWithoutValidation("If-Match", rev);
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
            if(requestType.IsGenericType)
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

        public virtual HttpRequest RemoveRequestType()
        {
            if (Headers.Contains(CustomHeaders.RequestType))
                Headers.Remove(CustomHeaders.RequestType);

            if (Headers.Contains(CustomHeaders.RequestEntityType))
                Headers.Remove(CustomHeaders.RequestEntityType);

            return this;
        }
    }
}