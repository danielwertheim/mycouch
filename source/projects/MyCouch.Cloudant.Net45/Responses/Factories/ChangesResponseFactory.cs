using System.Net.Http;
using EnsureThat;
using MyCouch.Responses.Factories.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class ChangesResponseFactory : ResponseFactoryBase
    {
        protected readonly ChangesResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public ChangesResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new ChangesResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual ChangesResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new ChangesResponse(), httpResponse, OnMaterializationOfSuccessfulResponseProperties, OnMaterializationOfFailedResponseProperties);
        }

        public virtual ChangesResponse<T> Create<T>(HttpResponseMessage httpResponse)
        {
            return Materialize(new ChangesResponse<T>(), httpResponse, OnMaterializationOfSuccessfulResponseProperties, OnMaterializationOfFailedResponseProperties);
        }

        protected virtual void OnMaterializationOfSuccessfulResponseProperties<T>(ChangesResponse<T> response, HttpResponseMessage httpResponse)
        {
            SuccessfulResponseMaterializer.Materialize(response, httpResponse);
        }

        protected virtual void OnMaterializationOfFailedResponseProperties<T>(ChangesResponse<T> response, HttpResponseMessage httpResponse)
        {
            FailedResponseMaterializer.Materialize(response, httpResponse);
        }
    }
}