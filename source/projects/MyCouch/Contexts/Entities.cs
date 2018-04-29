using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.EntitySchemes;
using MyCouch.Extensions;
using MyCouch.HttpRequestFactories;
using MyCouch.Requests;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Entities : ApiContextBase<IDbConnection>, IEntities
    {
        public ISerializer Serializer { get; }
        public IEntityReflector Reflector { get; }

        protected GetEntityHttpRequestFactory GetHttpRequestFactory { get; set; }
        protected PostEntityHttpRequestFactory PostHttpRequestFactory { get; set; }
        protected PutEntityHttpRequestFactory PutHttpRequestFactory { get; set; }
        protected DeleteEntityHttpRequestFactory DeleteHttpRequestFactory { get; set; }

        protected EntityResponseFactory EntityResponseFactory { get; set; }

        public Entities(IDbConnection connection, ISerializer serializer, IEntityReflector entityReflector)
            : base(connection)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));
            Ensure.Any.IsNotNull(entityReflector, nameof(entityReflector));

            Serializer = serializer;
            Reflector = entityReflector;
            EntityResponseFactory = new EntityResponseFactory(serializer, Reflector);
            GetHttpRequestFactory = new GetEntityHttpRequestFactory();
            PostHttpRequestFactory = new PostEntityHttpRequestFactory(Serializer);
            PutHttpRequestFactory = new PutEntityHttpRequestFactory(Reflector, Serializer);
            DeleteHttpRequestFactory = new DeleteEntityHttpRequestFactory(Reflector);
        }

        public virtual Task<GetEntityResponse<T>> GetAsync<T>(string id, string rev = null, CancellationToken cancellationToken = default) where T : class
        {
            return GetAsync<T>(new GetEntityRequest(id, rev), cancellationToken);
        }

        public virtual async Task<GetEntityResponse<T>> GetAsync<T>(GetEntityRequest request, CancellationToken cancellationToken = default) where T : class
        {
            var httpRequest = GetHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await EntityResponseFactory.CreateAsync<GetEntityResponse<T>, T>(res).ForAwait();
            }
        }

        public virtual Task<EntityResponse<T>> PostAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            return PostAsync(new PostEntityRequest<T>(entity), cancellationToken);
        }

        public virtual async Task<EntityResponse<T>> PostAsync<T>(PostEntityRequest<T> request, CancellationToken cancellationToken = default) where T : class
        {
            var httpRequest = PostHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await ProcessEntityResponseAsync(request, res).ForAwait();
            }
        }

        public virtual Task<EntityResponse<T>> PutAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            return PutAsync(new PutEntityRequest<T>(entity), cancellationToken);
        }

        public virtual Task<EntityResponse<T>> PutAsync<T>(string id, T entity, CancellationToken cancellationToken = default) where T : class
        {
            return PutAsync(new PutEntityRequest<T>(id, entity), cancellationToken);
        }

        public virtual Task<EntityResponse<T>> PutAsync<T>(string id, string rev, T entity, CancellationToken cancellationToken = default) where T : class
        {
            return PutAsync(new PutEntityRequest<T>(id, rev, entity), cancellationToken);
        }

        public virtual async Task<EntityResponse<T>> PutAsync<T>(PutEntityRequest<T> request, CancellationToken cancellationToken = default) where T : class
        {
            var httpRequest = PutHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await ProcessEntityResponseAsync(request, res).ForAwait();
            }
        }

        public virtual Task<EntityResponse<T>> DeleteAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            return DeleteAsync(new DeleteEntityRequest<T>(entity), cancellationToken);
        }

        public virtual async Task<EntityResponse<T>> DeleteAsync<T>(DeleteEntityRequest<T> request, CancellationToken cancellationToken = default) where T : class
        {
            var httpRequest = DeleteHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await ProcessEntityResponseAsync(request, res).ForAwait();
            }
        }

        protected virtual async Task<EntityResponse<T>> ProcessEntityResponseAsync<T>(PostEntityRequest<T> request, HttpResponseMessage response)
            where T : class
        {
            var entityResponse = await EntityResponseFactory.CreateAsync<T>(response).ForAwait();
            entityResponse.Content = request.Entity;

            if (entityResponse.IsSuccess)
            {
                Reflector.IdMember.SetValueTo(entityResponse.Content, entityResponse.Id);
                Reflector.RevMember.SetValueTo(entityResponse.Content, entityResponse.Rev);
            }

            return entityResponse;
        }

        protected virtual async Task<EntityResponse<T>> ProcessEntityResponseAsync<T>(PutEntityRequest<T> request, HttpResponseMessage response)
            where T : class
        {
            var entityResponse = await EntityResponseFactory.CreateAsync<T>(response).ForAwait();
            entityResponse.Content = request.Entity;

            if(!string.IsNullOrWhiteSpace(request.ExplicitId))
                Reflector.IdMember.SetValueTo(entityResponse.Content, entityResponse.Id);

            if (entityResponse.IsSuccess)
                Reflector.RevMember.SetValueTo(entityResponse.Content, entityResponse.Rev);

            return entityResponse;
        }

        protected virtual async Task<EntityResponse<T>> ProcessEntityResponseAsync<T>(DeleteEntityRequest<T> request, HttpResponseMessage response)
            where T : class
        {
            var entityResponse = await EntityResponseFactory.CreateAsync<T>(response).ForAwait();
            entityResponse.Content = request.Entity;

            if (entityResponse.IsSuccess)
                Reflector.RevMember.SetValueTo(entityResponse.Content, entityResponse.Rev);

            return entityResponse;
        }
    }
}