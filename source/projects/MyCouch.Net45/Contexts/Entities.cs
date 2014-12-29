using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.EntitySchemes;
using MyCouch.Extensions;
using MyCouch.HttpRequestFactories;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Entities : ApiContextBase<IDbConnection>, IEntities
    {
        public ISerializer Serializer { get; private set; }
        public IEntityReflector Reflector { get; private set;}

        protected GetEntityHttpRequestFactory GetHttpRequestFactory { get; set; }
        protected PostEntityHttpRequestFactory PostHttpRequestFactory { get; set; }
        protected PutEntityHttpRequestFactory PutHttpRequestFactory { get; set; }
        protected DeleteEntityHttpRequestFactory DeleteHttpRequestFactory { get; set; }
 
        protected EntityResponseFactory EntityResponseFactory { get; set; }

        public Entities(IDbConnection connection, ISerializer serializer, IEntityReflector entityReflector)
            : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();
            Ensure.That(entityReflector, "entityReflector").IsNotNull();

            Serializer = serializer;
            Reflector = entityReflector;
            EntityResponseFactory = new EntityResponseFactory(serializer, Reflector);
            GetHttpRequestFactory = new GetEntityHttpRequestFactory();
            PostHttpRequestFactory = new PostEntityHttpRequestFactory(Serializer);
            PutHttpRequestFactory = new PutEntityHttpRequestFactory(Reflector, Serializer);
            DeleteHttpRequestFactory = new DeleteEntityHttpRequestFactory(Reflector);
        }

        public virtual Task<GetEntityResponse<T>> GetAsync<T>(string id, string rev = null) where T : class
        {
            return GetAsync<T>(new GetEntityRequest(id, rev));
        }

        public virtual async Task<GetEntityResponse<T>> GetAsync<T>(GetEntityRequest request) where T : class
        {
            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessEntityResponse<T>(request, res);
            }
        }

        public virtual Task<EntityResponse<T>> PostAsync<T>(T entity) where T : class
        {
            return PostAsync(new PostEntityRequest<T>(entity));
        }

        public virtual async Task<EntityResponse<T>> PostAsync<T>(PostEntityRequest<T> request) where T : class
        {
            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessEntityResponse(request, res);
            }
        }

        public virtual Task<EntityResponse<T>> PutAsync<T>(T entity) where T : class
        {
            return PutAsync(new PutEntityRequest<T>(entity));
        }

        public virtual async Task<EntityResponse<T>> PutAsync<T>(PutEntityRequest<T> request) where T : class
        {
            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessEntityResponse(request, res);
            }
        }

        public virtual Task<EntityResponse<T>> DeleteAsync<T>(T entity) where T : class
        {
            return DeleteAsync(new DeleteEntityRequest<T>(entity));
        }

        public virtual async Task<EntityResponse<T>> DeleteAsync<T>(DeleteEntityRequest<T> request) where T : class
        {
            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessEntityResponse(request, res);
            }
        }

        protected virtual HttpRequest CreateHttpRequest(GetEntityRequest request)
        {
            return GetHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest<T>(PostEntityRequest<T> request) where T : class
        {
            return PostHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest<T>(PutEntityRequest<T> request) where T : class
        {
            return PutHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest<T>(DeleteEntityRequest<T> request) where T : class
        {
            return DeleteHttpRequestFactory.Create(request);
        }

        protected virtual GetEntityResponse<T> ProcessEntityResponse<T>(GetEntityRequest request, HttpResponseMessage response) where T : class
        {
            return EntityResponseFactory.Create<GetEntityResponse<T>, T>(response);
        }

        protected virtual EntityResponse<T> ProcessEntityResponse<T>(PostEntityRequest<T> request, HttpResponseMessage response) where T : class
        {
            var entityResponse = EntityResponseFactory.Create<T>(response);
            entityResponse.Content = request.Entity;

            if (entityResponse.IsSuccess)
            {
                Reflector.IdMember.SetValueTo(entityResponse.Content, entityResponse.Id);
                Reflector.RevMember.SetValueTo(entityResponse.Content, entityResponse.Rev);
            }

            return entityResponse;
        }

        protected virtual EntityResponse<T> ProcessEntityResponse<T>(PutEntityRequest<T> request, HttpResponseMessage response) where T : class
        {
            var entityResponse = EntityResponseFactory.Create<T>(response);
            entityResponse.Content = request.Entity;

            if (entityResponse.IsSuccess)
                Reflector.RevMember.SetValueTo(entityResponse.Content, entityResponse.Rev);

            return entityResponse;
        }

        protected virtual EntityResponse<T> ProcessEntityResponse<T>(DeleteEntityRequest<T> request, HttpResponseMessage response) where T : class
        {
            var entityResponse = EntityResponseFactory.Create<T>(response);
            entityResponse.Content = request.Entity;

            if (entityResponse.IsSuccess)
                Reflector.RevMember.SetValueTo(entityResponse.Content, entityResponse.Rev);

            return entityResponse;
        }
    }
}