using System.Net.Http;
using EnsureThat;
using MyCouch.Responses.Factories.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class EntityResponseFactory : ResponseFactoryBase
    {
        protected readonly EntityResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedEntityResponseMaterializer FailedResponseMaterializer;

        public EntityResponseFactory(ISerializer serializer, IEntitySerializer entitySerializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();
            Ensure.That(entitySerializer, "entitySerializer").IsNotNull();

            SuccessfulResponseMaterializer = new EntityResponseMaterializer(serializer, entitySerializer);
            FailedResponseMaterializer = new FailedEntityResponseMaterializer(serializer);
        }

        public virtual EntityResponse<T> Create<T>(HttpResponseMessage httpResponse) where T : class
        {
            return Materialize(new EntityResponse<T>(), httpResponse, OnMaterializationOfSuccessfulResponseProperties, OnMaterializationOfFailedResponseProperties);
        }

        protected virtual void OnMaterializationOfSuccessfulResponseProperties<T>(EntityResponse<T> response, HttpResponseMessage httpResponse) where T : class
        {
            SuccessfulResponseMaterializer.Materialize(response, httpResponse);
        }

        protected virtual void OnMaterializationOfFailedResponseProperties<T>(EntityResponse<T> response, HttpResponseMessage httpResponse) where T : class
        {
            FailedResponseMaterializer.Materialize(response, httpResponse);
        }
    }
}