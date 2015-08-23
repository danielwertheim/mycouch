using System.Net.Http;
using EnsureThat;
using MyCouch.Cloudant.Requests;
using MyCouch.Net;

namespace MyCouch.Cloudant.HttpRequestFactories
{
    public class GenerateApiKeyHttpRequestFactory
    {
        public virtual HttpRequest Create(GenerateApiKeyRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            return new HttpRequest(HttpMethod.Post, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType());
        }

        protected virtual string GenerateRelativeUrl(GenerateApiKeyRequest request)
        {
            return "/_api/v2/api_keys";
        }
    }
}