using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using MyCouch.Extensions;

namespace MyCouch.Net
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class HttpRequest : HttpRequestMessage
    {
        public static class CustomHeaders
        {
            public const string RequestType = "mycouch-type";
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

        public virtual HttpRequest SetContent(string content)
        {
            if(!string.IsNullOrWhiteSpace(content))
                Content = new JsonContent(content);

            return this;
        }

        public virtual HttpRequest SetContent(byte[] content, string contentType)
        {
            if (content != null && content.Length > 0)
                Content = new BytesContent(content, contentType);

            return this;
        }

        public virtual HttpRequest SetRequestType(Type requestType)
        {
            Headers.Add(CustomHeaders.RequestType, GetRequestTypeName(requestType));

            return this;
        }

        protected virtual string GetRequestTypeName(Type requestType)
        {
#if net45
            return requestType.IsGenericType
                ? string.Format("{0}:{1}", requestType.Name.Substring(0, requestType.Name.IndexOf('`')), requestType.GenericTypeArguments[0].Name)
                : requestType.Name;
#endif
#if net40
            return requestType.IsGenericType
                ? string.Format("{0}:{1}", requestType.Name.Substring(0, requestType.Name.IndexOf('`')), requestType.GetGenericArguments()[0].Name)
                : requestType.Name;
#endif
#if NETFX_CORE
            var typeInfo = requestType.GetTypeInfo();
            return typeInfo.IsGenericType
                ? string.Format("{0}:{1}", requestType.Name.Substring(0, requestType.Name.IndexOf('`')), requestType.GenericTypeArguments[0].Name)
                : requestType.Name;
#endif
        }

        public virtual HttpRequest RemoveRequestType()
        {
            if (Headers.Contains(CustomHeaders.RequestType))
                Headers.Remove(CustomHeaders.RequestType);

            return this;
        }
    }
}