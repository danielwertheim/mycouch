using System.Threading.Tasks;
using MyCouch.EnsureThat;
using MyCouch.Cloudant.HttpRequestFactories;
using MyCouch.Cloudant.Requests;
using MyCouch.Cloudant.Responses;
using MyCouch.Cloudant.Responses.Factories;
using MyCouch.Contexts;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Contexts
{
    public class Security : ApiContextBase<IServerConnection>, ISecurity
    {
        protected GenerateApiKeyHttpRequestFactory GenerateApiKeyHttpRequestFactory { get; set; }
        protected GenerateApiKeyResponseFactory GenerateApiKeyResponseFactory { get; set; }

        public Security(IServerConnection connection, ISerializer serializer)
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

            var httpRequest = GenerateApiKeyHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return await GenerateApiKeyResponseFactory.CreateAsync(res).ForAwait();
            }
        }
    }
}