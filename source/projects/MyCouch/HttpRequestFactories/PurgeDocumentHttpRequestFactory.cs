using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Serialization;

namespace MyCouch.HttpRequestFactories
{
    public class PurgeDocumentHttpRequestFactory
    {
        protected ISerializer Serializer { get; private set; }

        public PurgeDocumentHttpRequestFactory(ISerializer serializer)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            Serializer = serializer;
        }

        public virtual HttpRequest Create(PurgeDocumentRequest request)
        {
            Ensure.Any.IsNotNull(request, nameof(request));

            return new HttpRequest(HttpMethod.Post, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType())
                .SetJsonContent(Serializer.ToJson(request));
        }

        protected virtual string GenerateRelativeUrl(PurgeDocumentRequest request)
        {
            return "/_purge";
        }
    }
}