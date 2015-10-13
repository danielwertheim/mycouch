using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class ReplicationResponseFactory : ResponseFactoryBase
    {
        protected readonly SimpleDeserializingResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public ReplicationResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new SimpleDeserializingResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual ReplicationResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize<ReplicationResponse>(
                httpResponse,
                SuccessfulResponseMaterializer.Materialize,
                FailedResponseMaterializer.Materialize);
        }
    }
}