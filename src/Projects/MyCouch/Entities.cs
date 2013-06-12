using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Commands;
using MyCouch.Core;
using MyCouch.Net;

namespace MyCouch
{
    public class Entities : IEntities
    {
        protected readonly IClient Client;

        public Entities(IClient client)
        {
            Ensure.That(client, "Client").IsNotNull();

            Client = client;
        }

        public virtual EntityResponse<T> Get<T>(string id, string rev = null) where T : class
        {
            return Get<T>(new GetEntityCommand(id, rev));
        }

        public virtual Task<EntityResponse<T>> GetAsync<T>(string id, string rev = null) where T : class
        {
            return GetAsync<T>(new GetEntityCommand(id, rev));
        }

        public virtual EntityResponse<T> Get<T>(GetEntityCommand cmd) where T : class
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = Send(req);

            return ProcessEntityResponse<T>(cmd, res);
        }

        public virtual async Task<EntityResponse<T>> GetAsync<T>(GetEntityCommand cmd) where T : class
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = SendAsync(req);

            return ProcessEntityResponse<T>(cmd, await res.ForAwait());
        }

        public virtual EntityResponse<T> Post<T>(T entity) where T : class
        {
            return Post(new PostEntityCommand<T>(entity));
        }

        public virtual Task<EntityResponse<T>> PostAsync<T>(T entity) where T : class
        {
            return PostAsync(new PostEntityCommand<T>(entity));
        }

        public virtual EntityResponse<T> Post<T>(PostEntityCommand<T> cmd) where T : class
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = Send(req);

            return ProcessEntityResponse(cmd, res);
        }

        public virtual async Task<EntityResponse<T>> PostAsync<T>(PostEntityCommand<T> cmd) where T : class
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = SendAsync(req);

            return ProcessEntityResponse(cmd, await res.ForAwait());
        }

        public virtual EntityResponse<T> Put<T>(T entity) where T : class
        {
            return Put(new PutEntityCommand<T>(entity));
        }

        public virtual Task<EntityResponse<T>> PutAsync<T>(T entity) where T : class
        {
            return PutAsync(new PutEntityCommand<T>(entity));
        }

        public virtual EntityResponse<T> Put<T>(PutEntityCommand<T> cmd) where T : class
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = Send(req);

            return ProcessEntityResponse(cmd, res);
        }

        public virtual async Task<EntityResponse<T>> PutAsync<T>(PutEntityCommand<T> cmd) where T : class
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = SendAsync(req);

            return ProcessEntityResponse(cmd, await res.ForAwait());
        }

        public virtual EntityResponse<T> Delete<T>(T entity) where T : class
        {
            return Delete(new DeleteEntityCommand<T>(entity));
        }

        public virtual Task<EntityResponse<T>> DeleteAsync<T>(T entity) where T : class
        {
            return DeleteAsync(new DeleteEntityCommand<T>(entity));
        }

        public virtual EntityResponse<T> Delete<T>(DeleteEntityCommand<T> cmd) where T : class
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = Send(req);

            return ProcessEntityResponse(cmd, res);
        }

        public virtual async Task<EntityResponse<T>> DeleteAsync<T>(DeleteEntityCommand<T> cmd) where T : class
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = SendAsync(req);
            
            return ProcessEntityResponse(cmd, await res.ForAwait());
        }

        protected virtual HttpResponseMessage Send(HttpRequestMessage request)
        {
            return Client.Connection.Send(request);
        }

        protected virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return Client.Connection.SendAsync(request);
        }

        protected virtual string SerializeEntity<T>(T entity) where T : class
        {
            return Client.Serializer.SerializeEntity(entity);
        }

        protected virtual T DeserializeEntity<T>(string data) where T : class
        {
            return Client.Serializer.Deserialize<T>(data);
        }

        protected virtual HttpRequestMessage CreateRequest(GetEntityCommand cmd)
        {
            var req = new HttpRequest(HttpMethod.Get, GenerateRequestUrl(cmd.Id, cmd.Rev));

            req.SetIfMatch(cmd.Rev);

            return req;
        }

        protected virtual HttpRequestMessage CreateRequest<T>(PostEntityCommand<T> cmd) where T : class
        {
            var req = new HttpRequest(HttpMethod.Post, GenerateRequestUrl());

            req.SetContent(SerializeEntity(cmd.Entity));

            return req;
        }

        protected virtual HttpRequestMessage CreateRequest<T>(PutEntityCommand<T> cmd) where T : class
        {
            var id = Client.EntityReflector.IdMember.GetValueFrom(cmd.Entity);
            var rev = Client.EntityReflector.RevMember.GetValueFrom(cmd.Entity);
            var req = new HttpRequest(HttpMethod.Put, GenerateRequestUrl(id, rev));

            req.SetIfMatch(rev);
            req.SetContent(SerializeEntity(cmd.Entity));

            return req;
        }

        protected virtual HttpRequestMessage CreateRequest<T>(DeleteEntityCommand<T> cmd) where T : class
        {
            var id = Client.EntityReflector.IdMember.GetValueFrom(cmd.Entity);
            var rev = Client.EntityReflector.RevMember.GetValueFrom(cmd.Entity);
            var req = new HttpRequest(HttpMethod.Delete, GenerateRequestUrl(id, rev));

            req.SetIfMatch(rev);

            return req;
        }

        protected virtual string GenerateRequestUrl(string id = null, string rev = null)
        {
            return string.Format("{0}/{1}{2}",
                Client.Connection.Address,
                id ?? string.Empty,
                rev == null ? string.Empty : string.Concat("?rev=", rev));
        }

        protected virtual EntityResponse<T> ProcessEntityResponse<T>(GetEntityCommand cmd, HttpResponseMessage response) where T : class
        {
            return Client.ResponseFactory.CreateEntityResponse<T>(response);
        }

        protected virtual EntityResponse<T> ProcessEntityResponse<T>(PostEntityCommand<T> cmd, HttpResponseMessage response) where T : class
        {
            var entityResponse = Client.ResponseFactory.CreateEntityResponse<T>(response);
            entityResponse.Entity = cmd.Entity;

            if (entityResponse.IsSuccess)
            {
                Client.EntityReflector.IdMember.SetValueTo(entityResponse.Entity, entityResponse.Id);
                Client.EntityReflector.RevMember.SetValueTo(entityResponse.Entity, entityResponse.Rev);
            }

            return entityResponse;
        }

        protected virtual EntityResponse<T> ProcessEntityResponse<T>(PutEntityCommand<T> cmd, HttpResponseMessage response) where T : class
        {
            var entityResponse = Client.ResponseFactory.CreateEntityResponse<T>(response);
            entityResponse.Entity = cmd.Entity;

            if (entityResponse.IsSuccess)
                Client.EntityReflector.RevMember.SetValueTo(entityResponse.Entity, entityResponse.Rev);

            return entityResponse;
        }

        protected virtual EntityResponse<T> ProcessEntityResponse<T>(DeleteEntityCommand<T> cmd, HttpResponseMessage response) where T : class
        {
            var entityResponse = Client.ResponseFactory.CreateEntityResponse<T>(response);
            entityResponse.Entity = cmd.Entity;

            if (entityResponse.IsSuccess)
                Client.EntityReflector.RevMember.SetValueTo(entityResponse.Entity, entityResponse.Rev);

            return entityResponse;
        }
    }
}