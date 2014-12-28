using EnsureThat;
using MyCouch.Responses.Factories;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;
using System.Net.Http;

namespace MyCouch.Cloudant.Responses.Factories
{
    public class IndexResponseFactory : ResponseFactoryBase
    {
        protected readonly SimpleDeserializingResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;
        public IndexResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new SimpleDeserializingResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual IndexResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize<IndexResponse>(
                httpResponse,
                SuccessfulResponseMaterializer.Materialize,
                FailedResponseMaterializer.Materialize);
        }
    }
}
