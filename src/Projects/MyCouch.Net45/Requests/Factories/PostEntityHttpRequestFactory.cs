using System.Net.Http;
using MyCouch.EntitySchemes;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch.Requests.Factories
{
    public class PostEntityHttpRequestFactory : EntityHttpRequestFactoryBase
    {
        public PostEntityHttpRequestFactory(IConnection connection, IEntitySerializer serializer, IEntityReflector reflector)
            : base(connection, serializer, reflector) { }

        public virtual HttpRequest Create<T>(PostEntityRequest<T> request) where T : class
        {
            var httpRequest = CreateFor<PostEntityRequest<T>>(HttpMethod.Post, GenerateRequestUrl());

            httpRequest.SetContent(SerializeEntity(request.Entity));

            return httpRequest;
        }
    }
}