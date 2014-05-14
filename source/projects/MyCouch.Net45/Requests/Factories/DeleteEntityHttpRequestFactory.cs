using System.Net.Http;
using EnsureThat;
using MyCouch.EntitySchemes;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class DeleteEntityHttpRequestFactory
    {
        protected IEntityReflector Reflector { get; private set; }

        public DeleteEntityHttpRequestFactory(IEntityReflector reflector)
        {
            Ensure.That(reflector, "reflector").IsNotNull();

            Reflector = reflector;
        }

        public virtual HttpRequest Create<T>(DeleteEntityRequest<T> request) where T : class
        {
            Ensure.That(request, "request").IsNotNull();

            var entityId = GetEntityId(request);
            var entityRev = GetEntityRev(request);

            return new HttpRequest(HttpMethod.Delete, GenerateRelativeUrl(entityId, entityRev))
                .SetRequestTypeHeader(request.GetType())
                .SetIfMatchHeader(entityRev);
        }

        protected virtual string GetEntityId<T>(DeleteEntityRequest<T> request) where T : class
        {
            var entityId = Reflector.IdMember.GetValueFrom(request.Entity);

            Ensure.That(entityId, "request")
                .WithExtraMessageOf(() => "Could not extract entity Id from entity being deleted. Ensure member exists.")
                .IsNotNullOrWhiteSpace();

            return entityId;
        }

        protected virtual string GetEntityRev<T>(DeleteEntityRequest<T> request) where T : class
        {
            var entityRev = Reflector.RevMember.GetValueFrom(request.Entity);
            Ensure.That(entityRev, "request")
                .WithExtraMessageOf(() => "Could not extract entity rev from entity being deleted. Ensure member exists.")
                .IsNotNullOrWhiteSpace();
            return entityRev;
        }

        protected virtual string GenerateRelativeUrl(string entityId, string entityRev)
        {
            var urlParams = new UrlParams();

            urlParams.AddRequired("rev", entityRev);

            return string.Format("/{0}{1}", new UrlSegment(entityId), new QueryString(urlParams));
        }
    }
}