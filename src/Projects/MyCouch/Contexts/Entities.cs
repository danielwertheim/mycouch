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

        public virtual async Task<EntityResponse<T>> GetAsync<T>(GetEntityRequest cmd) where T : class
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            using (var req = CreateRequest(cmd))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessEntityResponse<T>(cmd, res);
                }
            }
        }

        public virtual Task<EntityResponse<T>> PostAsync<T>(T entity) where T : class
        {
            return PostAsync(new PostEntityRequest<T>(entity));
        }

        public virtual async Task<EntityResponse<T>> PostAsync<T>(PostEntityRequest<T> cmd) where T : class
        {
            Ensure.That(cmd, "cmd").IsNotNull();


            using (var req = CreateRequest(cmd))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessEntityResponse(cmd, res);
                }
            }
        }

        public virtual Task<EntityResponse<T>> PutAsync<T>(T entity) where T : class
        {
            return PutAsync(new PutEntityRequest<T>(entity));
        }

        public virtual async Task<EntityResponse<T>> PutAsync<T>(PutEntityRequest<T> cmd) where T : class
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            using (var req = CreateRequest(cmd))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessEntityResponse(cmd, res);
                }
            }
        }

        public virtual Task<EntityResponse<T>> DeleteAsync<T>(T entity) where T : class
        {
            return DeleteAsync(new DeleteEntityRequest<T>(entity));
        }

        public virtual async Task<EntityResponse<T>> DeleteAsync<T>(DeleteEntityRequest<T> cmd) where T : class
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            using (var req = CreateRequest(cmd))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessEntityResponse(cmd, res);
                }
            }
        }

        protected virtual string SerializeEntity<T>(T entity) where T : class
        {
            return Serializer.Serialize(entity);
        }

        protected virtual HttpRequestMessage CreateRequest(GetEntityRequest cmd)
        {
            var req = new HttpRequest(HttpMethod.Get, GenerateRequestUrl(cmd.Id, cmd.Rev));

            req.SetIfMatch(cmd.Rev);

            return req;
        }

        protected virtual HttpRequestMessage CreateRequest<T>(PostEntityRequest<T> cmd) where T : class
        {
            var req = new HttpRequest(HttpMethod.Post, GenerateRequestUrl());

            req.SetContent(SerializeEntity(cmd.Entity));

            return req;
        }

        protected virtual HttpRequestMessage CreateRequest<T>(PutEntityRequest<T> cmd) where T : class
        {
            var id = Reflector.IdMember.GetValueFrom(cmd.Entity);
            var rev = Reflector.RevMember.GetValueFrom(cmd.Entity);
            var req = new HttpRequest(HttpMethod.Put, GenerateRequestUrl(id, rev));

            req.SetIfMatch(rev);
            req.SetContent(SerializeEntity(cmd.Entity));

            return req;
        }

        protected virtual HttpRequestMessage CreateRequest<T>(DeleteEntityRequest<T> cmd) where T : class
        {
            var entityId = Reflector.IdMember.GetValueFrom(cmd.Entity);
            Ensure.That(entityId, "entityId").IsNotNullOrWhiteSpace();

            var entityRev = Reflector.RevMember.GetValueFrom(cmd.Entity);
            Ensure.That(entityRev, "entityRev").IsNotNullOrWhiteSpace();

            var req = new HttpRequest(HttpMethod.Delete, GenerateRequestUrl(entityId, entityRev));

            req.SetIfMatch(entityRev);

            return req;
        }

        protected virtual string GenerateRequestUrl(string id = null, string rev = null)
        {
            return string.Format("{0}/{1}{2}",
                Connection.Address,
                id ?? string.Empty,
                rev == null ? string.Empty : string.Concat("?rev=", rev));
        }

        protected virtual EntityResponse<T> ProcessEntityResponse<T>(GetEntityRequest cmd, HttpResponseMessage response) where T : class
        {
            return EntityResponseFactory.Create<T>(response);
        }

        protected virtual EntityResponse<T> ProcessEntityResponse<T>(PostEntityRequest<T> cmd, HttpResponseMessage response) where T : class
        {
            var entityResponse = EntityResponseFactory.Create<T>(response);
            entityResponse.Entity = cmd.Entity;

            if (entityResponse.IsSuccess)
            {
                Reflector.IdMember.SetValueTo(entityResponse.Entity, entityResponse.Id);
                Reflector.RevMember.SetValueTo(entityResponse.Entity, entityResponse.Rev);
            }

            return entityResponse;
        }

        protected virtual EntityResponse<T> ProcessEntityResponse<T>(PutEntityRequest<T> cmd, HttpResponseMessage response) where T : class
        {
            var entityResponse = EntityResponseFactory.Create<T>(response);
            entityResponse.Entity = cmd.Entity;

            if (entityResponse.IsSuccess)
                Reflector.RevMember.SetValueTo(entityResponse.Entity, entityResponse.Rev);

            return entityResponse;
        }

        protected virtual EntityResponse<T> ProcessEntityResponse<T>(DeleteEntityRequest<T> cmd, HttpResponseMessage response) where T : class
        {
            var entityResponse = EntityResponseFactory.Create<T>(response);
            entityResponse.Entity = cmd.Entity;

            if (entityResponse.IsSuccess)
                Reflector.RevMember.SetValueTo(entityResponse.Entity, entityResponse.Rev);

            return entityResponse;
        }
    }
}