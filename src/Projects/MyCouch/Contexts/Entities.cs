using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.EntitySchemes;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Requests.Factories;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Entities : ApiContextBase, IEntities
    {
        public IEntitySerializer Serializer { get; private set; }
        public IEntityReflector Reflector { get; private set;}
        protected GetEntityHttpRequestFactory GetEntityHttpRequestFactory { get; set; }
        protected PostEntityHttpRequestFactory PostEntityHttpRequestFactory { get; set; }
        protected PutEntityHttpRequestFactory PutEntityHttpRequestFactory { get; set; }
        protected DeleteEntityHttpRequestFactory DeleteEntityHttpRequestFactory { get; set; }
 
        protected EntityResponseFactory EntityResponseFactory { get; set; }

        public Entities(IConnection connection, ISerializer serializer, IEntitySerializer entitySerializer, IEntityReflector entityReflector)
            : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();
            Ensure.That(entitySerializer, "entitySerializer").IsNotNull();
            Ensure.That(entityReflector, "entityReflector").IsNotNull();

            Serializer = entitySerializer;
            EntityResponseFactory = new EntityResponseFactory(serializer, entitySerializer);
            Reflector = entityReflector;
            GetEntityHttpRequestFactory = new GetEntityHttpRequestFactory(Connection, Serializer, Reflector);
            PostEntityHttpRequestFactory = new PostEntityHttpRequestFactory(Connection, Serializer, Reflector);
            PutEntityHttpRequestFactory = new PutEntityHttpRequestFactory(Connection, Serializer, Reflector);
            DeleteEntityHttpRequestFactory = new DeleteEntityHttpRequestFactory(Connection, Serializer, Reflector);
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

        protected virtual HttpRequest CreateHttpRequest(GetEntityRequest request)
        {
            return GetEntityHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest<T>(PostEntityRequest<T> request) where T : class
        {
            return PostEntityHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest<T>(PutEntityRequest<T> request) where T : class
        {
            return PutEntityHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest<T>(DeleteEntityRequest<T> request) where T : class
        {
            return DeleteEntityHttpRequestFactory.Create(request);
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