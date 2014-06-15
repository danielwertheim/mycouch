using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Cloudant.HttpRequestFactories;
using MyCouch.Cloudant.Requests;
using MyCouch.Cloudant.Responses;
using MyCouch.Cloudant.Responses.Factories;
using MyCouch.Contexts;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Contexts
{
    public class Security : ApiContextBase<IServerClientConnection>, ISecurity
    {
        protected GenerateApiKeyHttpRequestFactory GenerateApiKeyHttpRequestFactory { get; set; }
        protected GenerateApiKeyResponseFactory GenerateApiKeyResponseFactory { get; set; }

        public Security(IServerClientConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            GenerateApiKeyHttpRequestFactory = new GenerateApiKeyHttpRequestFactory();
            GenerateApiKeyResponseFactory = new GenerateApiKeyResponseFactory(serializer);
        }

        public virtual Task<GenerateApiKeyResponse> GenerateApiKey()
        {
            return GenerateApiKey(new GenerateApiKeyRequest());
        }

        public virtual async Task<GenerateApiKeyResponse> GenerateApiKey(GenerateApiKeyRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessHttpResponse(res);
            }
        }

        protected virtual HttpRequest CreateHttpRequest(GenerateApiKeyRequest request)
        {
            return GenerateApiKeyHttpRequestFactory.Create(request);
        }

        protected virtual GenerateApiKeyResponse ProcessHttpResponse(HttpResponseMessage response)
        {
            return GenerateApiKeyResponseFactory.Create(response);
        }
    }
}