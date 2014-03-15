using System.Net.Http;
using EnsureThat;
using MyCouch.Responses.Factories.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class ViewQueryResponseFactory : ResponseFactoryBase
    {
        protected readonly ViewQueryResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public ViewQueryResponseFactory(ISerializer serializer, IEntitySerializer entitySerializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();
            Ensure.That(entitySerializer, "entitySerializer").IsNotNull();

            SuccessfulResponseMaterializer = new ViewQueryResponseMaterializer(entitySerializer);
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual ViewQueryResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(
                new ViewQueryResponse(),
                httpResponse,
                OnMaterializationOfSuccessfulResponseProperties,
                OnMaterializationOfFailedResponseProperties);
        }

        public virtual ViewQueryResponse<T> Create<T>(HttpResponseMessage httpResponse)
        {
            return Materialize(
                new ViewQueryResponse<T>(),
                httpResponse,
                OnMaterializationOfSuccessfulResponseProperties,
                OnMaterializationOfFailedResponseProperties);
        }

        public virtual ViewQueryResponse<TValue, TIncludedDoc> Create<TValue, TIncludedDoc>(HttpResponseMessage httpResponse)
        {
            return Materialize(
                new ViewQueryResponse<TValue, TIncludedDoc>(),
                httpResponse,
                OnMaterializationOfSuccessfulResponseProperties,
                OnMaterializationOfFailedResponseProperties);
        }

        protected virtual void OnMaterializationOfSuccessfulResponseProperties<TValue, TIncludedDoc>(ViewQueryResponse<TValue, TIncludedDoc> response, HttpResponseMessage httpResponse)
        {
            SuccessfulResponseMaterializer.Materialize(response, httpResponse);
        }

        protected virtual void OnMaterializationOfFailedResponseProperties<TValue, TIncludedDoc>(ViewQueryResponse<TValue, TIncludedDoc> response, HttpResponseMessage httpResponse)
        {
            FailedResponseMaterializer.Materialize(response, httpResponse);
        }
    }
}