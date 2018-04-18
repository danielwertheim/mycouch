using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MyCouch.EntitySchemes;
using MyCouch.Extensions;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class EntityResponseFactory : ResponseFactoryBase
    {
        protected readonly EntityResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedEntityResponseMaterializer FailedResponseMaterializer;

        public EntityResponseFactory(ISerializer serializer, IEntityReflector entityReflector)
        {
            SuccessfulResponseMaterializer = new EntityResponseMaterializer(serializer, entityReflector);
            FailedResponseMaterializer = new FailedEntityResponseMaterializer(serializer);
        }

        public virtual async Task<EntityResponse<TContent>> CreateAsync<TContent>(HttpResponseMessage httpResponse, CancellationToken cancellationToken = default)
            where TContent : class
        {
            return await MaterializeAsync<EntityResponse<TContent>>(
                httpResponse,
                SuccessfulResponseMaterializer.MaterializeAsync,
                FailedResponseMaterializer.MaterializeAsync, cancellationToken).ForAwait();
        }

        public virtual async Task<TResponse> CreateAsync<TResponse, TContent>(HttpResponseMessage httpResponse, CancellationToken cancellationToken = default)
            where TResponse : EntityResponse<TContent>, new()
            where TContent : class
        {
            return await MaterializeAsync<TResponse>(
                httpResponse,
                SuccessfulResponseMaterializer.MaterializeAsync,
                FailedResponseMaterializer.MaterializeAsync, cancellationToken).ForAwait();
        }
    }
}