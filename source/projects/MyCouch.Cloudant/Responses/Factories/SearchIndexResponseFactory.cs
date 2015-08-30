using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Cloudant.Responses.Materializers;
using MyCouch.Responses.Factories;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Responses.Factories
{
    public class SearchIndexResponseFactory : ResponseFactoryBase
    {
        protected readonly SearchIndexResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public SearchIndexResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new SearchIndexResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual SearchIndexResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize<SearchIndexResponse>(
                httpResponse,
                SuccessfulResponseMaterializer.Materialize,
                FailedResponseMaterializer.Materialize);
        }

        public virtual SearchIndexResponse<TIncludedDoc> Create<TIncludedDoc>(HttpResponseMessage httpResponse)
        {
            return Materialize<SearchIndexResponse<TIncludedDoc>>(
                httpResponse,
                SuccessfulResponseMaterializer.Materialize,
                FailedResponseMaterializer.Materialize);
        }
    }
}