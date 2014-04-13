using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.EntitySchemes;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch.Requests.Factories
{
    public class PutEntityHttpRequestFactory : EntityHttpRequestFactoryBase
    {
        public PutEntityHttpRequestFactory(IDbClientConnection connection, IEntitySerializer serializer, IEntityReflector reflector)
            : base(connection, serializer, reflector) {}

        public virtual HttpRequest Create<T>(PutEntityRequest<T> request) where T : class
        {
            Ensure.That(request, "request").IsNotNull();

            var id = Reflector.IdMember.GetValueFrom(request.Entity);
            var rev = Reflector.RevMember.GetValueFrom(request.Entity);
            var httpRequest = CreateFor<PutEntityRequest<T>>(HttpMethod.Put, GenerateRequestUrl(id, rev));

            httpRequest.SetIfMatch(rev);
            httpRequest.SetJsonContent(SerializeEntity(request.Entity));

            return httpRequest;
        }

        protected override string GenerateRequestUrl(string id = null, string rev = null)
        {
            Ensure.That(id, "id")
                .WithExtraMessageOf(() => ExceptionStrings.PutRequestIsMissingIdInUrl)
                .IsNotNullOrWhiteSpace();

            return base.GenerateRequestUrl(id, rev);
        }
    }
}