using System;
using System.Net.Http;
using EnsureThat;
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

        public virtual GenerateApiKeyResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize<GenerateApiKeyResponse>(
                httpResponse,
                MaterializeSuccessfulResponse,
                FailedResponseMaterializer.Materialize);
        }

        private async void MaterializeSuccessfulResponse(GenerateApiKeyResponse response, HttpResponseMessage httpResponse)
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