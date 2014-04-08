using System.Net.Http;
using EnsureThat;
using MyCouch.EntitySchemes;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch.Requests.Factories
{
    public class PostEntityHttpRequestFactory : EntityHttpRequestFactoryBase
    {
        public PostEntityHttpRequestFactory(IDbClientConnection connection, IEntitySerializer serializer, IEntityReflector reflector)
            : base(connection, serializer, reflector) { }

        public virtual HttpRequest Create<T>(PostEntityRequest<T> request) where T : class
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateFor<PostEntityRequest<T>>(HttpMethod.Post, GenerateRequestUrl());

            httpRequest.SetJsonContent(SerializeEntity(request.Entity));

            return httpRequest;
        }
    }
}