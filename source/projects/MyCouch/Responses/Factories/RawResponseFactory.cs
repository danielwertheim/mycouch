using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class RawResponseFactory : ResponseFactoryBase
    {
        protected readonly RawResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public RawResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new RawResponseMaterializer();
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual RawResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize<RawResponse>(
                httpResponse,
                SuccessfulResponseMaterializer.Materialize,
                FailedResponseMaterializer.Materialize);
        }
    }
}