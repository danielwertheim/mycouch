using System.Net.Http;
using EnsureThat;
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

        public SearchIndexResponseFactory(ISerializer serializer, IEntitySerializer entitySerializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();
            Ensure.That(entitySerializer, "entitySerializer").IsNotNull();

            SuccessfulResponseMaterializer = new SearchIndexResponseMaterializer(entitySerializer);
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual SearchIndexResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(
                new SearchIndexResponse(),
                httpResponse,
                OnMaterializationOfSuccessfulResponseProperties,
                OnMaterializationOfFailedResponseProperties);
        }

        public virtual SearchIndexResponse<TIncludedDoc> Create<TIncludedDoc>(HttpResponseMessage httpResponse)
        {
            return Materialize(
                new SearchIndexResponse<TIncludedDoc>(),
                httpResponse,
                OnMaterializationOfSuccessfulResponseProperties,
                OnMaterializationOfFailedResponseProperties);
        }

        protected virtual void OnMaterializationOfSuccessfulResponseProperties<TIncludedDoc>(SearchIndexResponse<TIncludedDoc> response, HttpResponseMessage httpResponse)
        {
            SuccessfulResponseMaterializer.Materialize(response, httpResponse);
        }

        protected virtual void OnMaterializationOfFailedResponseProperties<TIncludedDoc>(SearchIndexResponse<TIncludedDoc> response, HttpResponseMessage httpResponse)
        {
            FailedResponseMaterializer.Materialize(response, httpResponse);
        }
    }
}