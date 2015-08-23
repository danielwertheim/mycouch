using EnsureThat;
using MyCouch.Cloudant.Responses.Materializers;
using MyCouch.Responses.Factories;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;
using System.Net.Http;

namespace MyCouch.Cloudant.Responses.Factories
{
    public class FindResponseFactory : ResponseFactoryBase
    {
        protected readonly FindResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public FindResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new FindResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual FindResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize<FindResponse>(
                httpResponse,
                SuccessfulResponseMaterializer.Materialize,
                FailedResponseMaterializer.Materialize);
        }

        public virtual FindResponse<TIncludedDoc> Create<TIncludedDoc>(HttpResponseMessage httpResponse)
        {
            return Materialize<FindResponse<TIncludedDoc>>(
                httpResponse,
                SuccessfulResponseMaterializer.Materialize,
                FailedResponseMaterializer.Materialize);
        }
    }
}
