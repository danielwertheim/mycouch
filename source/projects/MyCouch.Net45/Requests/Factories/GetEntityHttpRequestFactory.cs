using System.Net.Http;
using EnsureThat;
using MyCouch.EntitySchemes;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch.Requests.Factories
{
    public class GetEntityHttpRequestFactory : EntityHttpRequestFactoryBase
    {
        public GetEntityHttpRequestFactory(IDbClientConnection connection, IEntitySerializer serializer, IEntityReflector reflector)
            : base(connection, serializer, reflector) {}

        public virtual HttpRequest Create(GetEntityRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateFor<GetEntityRequest>(HttpMethod.Get, GenerateRequestUrl(request.Id, request.Rev));

            httpRequest.SetIfMatch(request.Rev);

            return httpRequest;
        }
    }
}