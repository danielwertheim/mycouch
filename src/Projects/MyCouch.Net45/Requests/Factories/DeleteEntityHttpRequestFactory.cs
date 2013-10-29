using System.Net.Http;
using EnsureThat;
using MyCouch.EntitySchemes;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch.Requests.Factories
{
    public class DeleteEntityHttpRequestFactory : EntityHttpRequestFactoryBase
    {
        public DeleteEntityHttpRequestFactory(IConnection connection, IEntitySerializer serializer, IEntityReflector reflector)
            : base(connection, serializer, reflector) {}

        public virtual HttpRequest Create<T>(DeleteEntityRequest<T> request) where T : class
        {
            var entityId = Reflector.IdMember.GetValueFrom(request.Entity);
            Ensure.That(entityId, "entityId").IsNotNullOrWhiteSpace();

            var entityRev = Reflector.RevMember.GetValueFrom(request.Entity);
            Ensure.That(entityRev, "entityRev").IsNotNullOrWhiteSpace();

            var httpRequest = CreateFor<DeleteEntityRequest<T>>(HttpMethod.Delete, GenerateRequestUrl(entityId, entityRev));

            httpRequest.SetIfMatch(entityRev);

            return httpRequest;
        }
    }
}