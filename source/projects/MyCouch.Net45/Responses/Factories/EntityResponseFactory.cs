using System.Net.Http;
using EnsureThat;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class EntityResponseFactory : ResponseFactoryBase
    {
        protected readonly EntityResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedEntityResponseMaterializer FailedResponseMaterializer;

        public EntityResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new EntityResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedEntityResponseMaterializer(serializer);
        }

        public virtual EntityResponse<TContent> Create<TContent>(HttpResponseMessage httpResponse) where TContent : class
        {
            return Materialize<EntityResponse<TContent>>(
                httpResponse,
                MaterializeSuccessfulResponse,
                MaterializeFailedResponse);
        }

        public virtual TResponse Create<TResponse, TContent>(HttpResponseMessage httpResponse)
            where TResponse : EntityResponse<TContent>, new()
            where TContent : class
        {
            return Materialize<TResponse>(
                httpResponse,
                MaterializeSuccessfulResponse,
                MaterializeFailedResponse);
        }

        protected virtual void MaterializeSuccessfulResponse<T>(EntityResponse<T> response, HttpResponseMessage httpResponse) where T : class
        {
            SuccessfulResponseMaterializer.Materialize(response, httpResponse);
        }

        protected virtual void MaterializeFailedResponse<T>(EntityResponse<T> response, HttpResponseMessage httpResponse) where T : class
        {
            FailedResponseMaterializer.Materialize(response, httpResponse);
        }
    }
}