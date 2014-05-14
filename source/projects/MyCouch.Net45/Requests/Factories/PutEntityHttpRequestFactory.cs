using System.Net.Http;
using EnsureThat;
using MyCouch.EntitySchemes;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch.Requests.Factories
{
    public class PutEntityHttpRequestFactory
    {
        protected IEntityReflector Reflector { get; private set; }
        protected IEntitySerializer Serializer { get; private set; }

        protected PutEntityHttpRequestFactory(IEntityReflector reflector, IEntitySerializer serializer)
        {
            Ensure.That(reflector, "reflector").IsNotNull();
            Ensure.That(serializer, "serializer").IsNotNull();

            Reflector = reflector;
            Serializer = serializer;
        }

        public virtual HttpRequest Create<T>(PutEntityRequest<T> request) where T : class
        {
            Ensure.That(request, "request").IsNotNull();

            var entityId = GetEntityId(request);
            var entityRev = GetEntityRev(request);

            return new HttpRequest(HttpMethod.Put, GenerateRelativeUrl(entityId, entityRev, request.Batch))
                .SetRequestTypeHeader(request.GetType())
                .SetIfMatchHeader(entityRev)
                .SetJsonContent(SerializeEntity(request.Entity));
        }

        protected virtual string GetEntityId<T>(PutEntityRequest<T> request) where T : class
        {
            var entityId = Reflector.IdMember.GetValueFrom(request.Entity);

            Ensure.That(entityId, "request")
                .WithExtraMessageOf(() => "Could not extract entity Id from entity being deleted. Ensure member exists.")
                .IsNotNullOrWhiteSpace();

            return entityId;
        }

        protected virtual string GetEntityRev<T>(PutEntityRequest<T> request) where T : class
        {
            return Reflector.RevMember.GetValueFrom(request.Entity);
        }

        protected virtual string GenerateRelativeUrl(string entityId, string entityRev, bool batch)
        {
            var urlParams = new UrlParams
            {
                new UrlParam("rev", entityRev)
            };
            urlParams.AddIfTrue("batch", batch);

            return string.Format("/{0}{1}", new UrlSegment(entityId), new QueryString(urlParams));
        }

        protected virtual string SerializeEntity<T>(T entity) where T : class
        {
            return Serializer.Serialize(entity);
        }
    }
}