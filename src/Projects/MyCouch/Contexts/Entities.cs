using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.EntitySchemes;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Entities : ApiContextBase, IEntities
    {
        public ISerializer Serializer { get; protected set; }
        public IEntityReflector Reflector { get; protected set;}
        protected EntityResponseFactory EntityResponseFactory { get; set; }

        public Entities(IConnection connection, SerializationConfiguration serializationConfiguration, IEntityReflector entityReflector)
            : base(connection)
        {
            Ensure.That(serializationConfiguration, "serializationConfiguration").IsNotNull();
            Ensure.That(entityReflector, "entityReflector").IsNotNull();

            Serializer = new EntitySerializer(serializationConfiguration);
            EntityResponseFactory = new EntityResponseFactory(serializationConfiguration, Serializer);
            Reflector = entityReflector;
        }

        public virtual Task<EntityResponse<T>> GetAsync<T>(string id, string rev = null) where T : class
        {
            return GetAsync<T>(new GetEntityRequest(id, rev));
        }

        public virtual async Task<EntityResponse<T>> GetAsync<T>(GetEntityRequest request) where T : class
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessEntityResponse<T>(request, res);
                }
            }
        }

        public virtual Task<EntityResponse<T>> PostAsync<T>(T entity) where T : class
        {
            return PostAsync(new PostEntityRequest<T>(entity));
        }

        public virtual async Task<EntityResponse<T>> PostAsync<T>(PostEntityRequest<T> request) where T : class
        {
            Ensure.That(request, "request").IsNotNull();


            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessEntityResponse(request, res);
                }
            }
        }

        public virtual Task<EntityResponse<T>> PutAsync<T>(T entity) where T : class
        {
            return PutAsync(new PutEntityRequest<T>(entity));
        }

        public virtual async Task<EntityResponse<T>> PutAsync<T>(PutEntityRequest<T> request) where T : class
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessEntityResponse(request, res);
                }
            }
        }

        public virtual Task<EntityResponse<T>> DeleteAsync<T>(T entity) where T : class
        {
            return DeleteAsync(new DeleteEntityRequest<T>(entity));
        }

        public virtual async Task<EntityResponse<T>> DeleteAsync<T>(DeleteEntityRequest<T> request) where T : class
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessEntityResponse(request, res);
                }
            }
        }

        protected virtual string SerializeEntity<T>(T entity) where T : class
        {
            return Serializer.Serialize(entity);
        }

        protected virtual HttpRequest CreateHttpRequest(GetEntityRequest request)
        {
            var httpRequest = new HttpRequest(HttpMethod.Get, GenerateRequestUrl(request.Id, request.Rev));

            httpRequest.SetIfMatch(request.Rev);

            return httpRequest;
        }

        protected virtual HttpRequest CreateHttpRequest<T>(PostEntityRequest<T> request) where T : class
        {
            var httpRequest = new HttpRequest(HttpMethod.Post, GenerateRequestUrl());

            httpRequest.SetContent(SerializeEntity(request.Entity));

            return httpRequest;
        }

        protected virtual HttpRequest CreateHttpRequest<T>(PutEntityRequest<T> request) where T : class
        {
            var id = Reflector.IdMember.GetValueFrom(request.Entity);
            var rev = Reflector.RevMember.GetValueFrom(request.Entity);
            var httpRequest = new HttpRequest(HttpMethod.Put, GenerateRequestUrl(id, rev));

            httpRequest.SetIfMatch(rev);
            httpRequest.SetContent(SerializeEntity(request.Entity));

            return httpRequest;
        }

        protected virtual HttpRequest CreateHttpRequest<T>(DeleteEntityRequest<T> request) where T : class
        {
            var entityId = Reflector.IdMember.GetValueFrom(request.Entity);
            Ensure.That(entityId, "entityId").IsNotNullOrWhiteSpace();

            var entityRev = Reflector.RevMember.GetValueFrom(request.Entity);
            Ensure.That(entityRev, "entityRev").IsNotNullOrWhiteSpace();

            var httpRequest = new HttpRequest(HttpMethod.Delete, GenerateRequestUrl(entityId, entityRev));

            httpRequest.SetIfMatch(entityRev);

            return httpRequest;
        }

        protected virtual string GenerateRequestUrl(string id = null, string rev = null)
        {
            return string.Format("{0}/{1}{2}",
                Connection.Address,
                id ?? string.Empty,
                rev == null ? string.Empty : string.Concat("?rev=", rev));
        }

        protected virtual EntityResponse<T> ProcessEntityResponse<T>(GetEntityRequest request, HttpResponseMessage response) where T : class
        {
            return EntityResponseFactory.Create<T>(response);
        }

        protected virtual EntityResponse<T> ProcessEntityResponse<T>(PostEntityRequest<T> request, HttpResponseMessage response) where T : class
        {
            var entityResponse = EntityResponseFactory.Create<T>(response);
            entityResponse.Entity = request.Entity;

            if (entityResponse.IsSuccess)
            {
                Reflector.IdMember.SetValueTo(entityResponse.Entity, entityResponse.Id);
                Reflector.RevMember.SetValueTo(entityResponse.Entity, entityResponse.Rev);
            }

            return entityResponse;
        }

        protected virtual EntityResponse<T> ProcessEntityResponse<T>(PutEntityRequest<T> request, HttpResponseMessage response) where T : class
        {
            var entityResponse = EntityResponseFactory.Create<T>(response);
            entityResponse.Entity = request.Entity;

            if (entityResponse.IsSuccess)
                Reflector.RevMember.SetValueTo(entityResponse.Entity, entityResponse.Rev);

            return entityResponse;
        }

        protected virtual EntityResponse<T> ProcessEntityResponse<T>(DeleteEntityRequest<T> request, HttpResponseMessage response) where T : class
        {
            var entityResponse = EntityResponseFactory.Create<T>(response);
            entityResponse.Entity = request.Entity;

            if (entityResponse.IsSuccess)
                Reflector.RevMember.SetValueTo(entityResponse.Entity, entityResponse.Rev);

            return entityResponse;
        }
    }
}