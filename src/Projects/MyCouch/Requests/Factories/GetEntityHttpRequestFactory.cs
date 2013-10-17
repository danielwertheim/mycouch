using System.Net.Http;
using MyCouch.EntitySchemes;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch.Requests.Factories
{
    public class GetEntityHttpRequestFactory : EntityHttpRequestFactoryBase, IHttpRequestFactory<GetEntityRequest>
    {
        public GetEntityHttpRequestFactory(IConnection connection, IEntitySerializer serializer, IEntityReflector reflector)
            : base(connection, serializer, reflector) {}

        public virtual HttpRequest Create(GetEntityRequest request)
        {
            var httpRequest = CreateFor<GetEntityRequest>(HttpMethod.Get, GenerateRequestUrl(request.Id, request.Rev));

            httpRequest.SetIfMatch(request.Rev);

            return httpRequest;
        }
    }
}