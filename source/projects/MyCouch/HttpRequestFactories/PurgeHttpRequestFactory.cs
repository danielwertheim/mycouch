using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Serialization;

namespace MyCouch.HttpRequestFactories
{
    public class PurgeHttpRequestFactory
    {
        protected ISerializer Serializer { get; private set; }

        public PurgeHttpRequestFactory(ISerializer serializer)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            Serializer = serializer;
        }

        public virtual HttpRequest Create(PurgeRequest request)
        {
            Ensure.Any.IsNotNull(request, nameof(request));

            var data = new PurgeData(request.SeqsById);

            return new HttpRequest(HttpMethod.Post, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType())
                .SetJsonContent(Serializer.ToJson(data));
        }

        protected virtual string GenerateRelativeUrl(PurgeRequest request)
        {
            return "/_purge";
        }
    }
}