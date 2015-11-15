using System;
using System.Net.Http;
using System.Threading.Tasks;
using MyCouch.EnsureThat;
using MyCouch.Extensions;
using MyCouch.Responses.Factories;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Responses.Factories
{
    public class GenerateApiKeyResponseFactory : ResponseFactoryBase
    {
        protected readonly ISerializer Serializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public GenerateApiKeyResponseFactory(ISerializer serializer)
        {
            Serializer = serializer;
            Ensure.That(serializer, "serializer").IsNotNull();

            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual async Task<GenerateApiKeyResponse> CreateAsync(HttpResponseMessage httpResponse)
        {
            return await MaterializeAsync<GenerateApiKeyResponse>(
                httpResponse,
                MaterializeSuccessfulResponseAsync,
                FailedResponseMaterializer.MaterializeAsync).ForAwait();
        }

        private async Task MaterializeSuccessfulResponseAsync(GenerateApiKeyResponse response, HttpResponseMessage httpResponse)
        {
            if (response.RequestMethod != HttpMethod.Post)
                throw new ArgumentException(GetType().Name + " only supports materializing POST responses.");

            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                Serializer.Populate(response, content);
            }
        }
    }
}