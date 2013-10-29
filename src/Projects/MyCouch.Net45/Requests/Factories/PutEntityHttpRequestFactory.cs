using System.Net.Http;
using MyCouch.EntitySchemes;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch.Requests.Factories
{
    public class PutEntityHttpRequestFactory : EntityHttpRequestFactoryBase
    {
        public PutEntityHttpRequestFactory(IConnection connection, IEntitySerializer serializer, IEntityReflector reflector)
            : base(connection, serializer, reflector) {}

        public virtual HttpRequest Create<T>(PutEntityRequest<T> request) where T : class
        {
            var id = Reflector.IdMember.GetValueFrom(request.Entity);
            var rev = Reflector.RevMember.GetValueFrom(request.Entity);
            var httpRequest = CreateFor<PutEntityRequest<T>>(HttpMethod.Put, GenerateRequestUrl(id, rev));

            httpRequest.SetIfMatch(rev);
            httpRequest.SetContent(SerializeEntity(request.Entity));

            return httpRequest;
        }
    }
}