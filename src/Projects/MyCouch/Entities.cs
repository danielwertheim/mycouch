using System;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Commands;
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
            return GetAsync<T>(new GetEntityCommand(id, rev)).Result;
        }

        public virtual Task<EntityResponse<T>> GetAsync<T>(string id, string rev = null) where T : class
        {
            return GetAsync<T>(new GetEntityCommand(id, rev));
        }

        public virtual EntityResponse<T> Get<T>(GetEntityCommand cmd) where T : class
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            return GetAsync<T>(cmd).Result;
        }

        public virtual async Task<EntityResponse<T>> GetAsync<T>(GetEntityCommand cmd) where T : class
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = SendAsync(req);

            return await ProcessEntityResponseAsync<T>(res);
        }

        public virtual EntityResponse<T> Post<T>(T entity) where T : class
        {
            return PostAsync(entity).Result;
        }

        public virtual async Task<EntityResponse<T>> PostAsync<T>(T entity) where T : class
        {
            Ensure.That(entity, "entity").IsNotNull();

            var req = CreateRequest(
                HttpMethod.Post,
                new EntityCommand
                {
                    Json = SerializeEntity(entity)
                });

            var res = SendAsync(req);
            var response = await ProcessEntityResponseAsync<T>(res);
            response.Entity = entity;

            if (response.IsSuccess)
            {
                Client.EntityReflector.IdMember.SetValueTo(response.Entity, response.Id);
                Client.EntityReflector.RevMember.SetValueTo(response.Entity, response.Rev);
            }

            return response;
        }

        public virtual EntityResponse<T> Put<T>(T entity) where T : class
        {
            return PutAsync(entity).Result;
        }

        public virtual async Task<EntityResponse<T>> PutAsync<T>(T entity) where T : class
        {
            Ensure.That(entity, "entity").IsNotNull();

            var req = CreateRequest(
                HttpMethod.Put,
                new EntityCommand
                {
                    Id = Client.EntityReflector.IdMember.GetValueFrom(entity),
                    Rev = Client.EntityReflector.RevMember.GetValueFrom(entity),
                    Json = SerializeEntity(entity)
                });
            var res = SendAsync(req);
            var response = await ProcessEntityResponseAsync<T>(res);
            response.Entity = entity;

            if (response.IsSuccess)
                Client.EntityReflector.RevMember.SetValueTo(response.Entity, response.Rev);

            return response;
        }

        public virtual EntityResponse<T> Delete<T>(T entity) where T : class
        {
            return DeleteAsync(entity).Result;
        }

        public virtual async Task<EntityResponse<T>> DeleteAsync<T>(T entity) where T : class
        {
            Ensure.That(entity, "entity").IsNotNull();

            var req = CreateRequest(
                HttpMethod.Delete,
                new EntityCommand
                {
                    Id = Client.EntityReflector.IdMember.GetValueFrom(entity),
                    Rev = Client.EntityReflector.RevMember.GetValueFrom(entity)
                });
            var res = SendAsync(req);
            var response = await ProcessEntityResponseAsync<T>(res);
            response.Entity = entity;

            if (response.IsSuccess)
                Client.EntityReflector.RevMember.SetValueTo(response.Entity, response.Rev);

            return response;
        }

        protected virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return Client.Connection.SendAsync(request);
        }

        protected virtual string SerializeEntity<T>(T entity) where T : class
        {
            return Client.Serializer.SerializeEntity(entity);
        }

        protected virtual T Deserialize<T>(string data) where T : class
        {
            return Client.Serializer.Deserialize<T>(data);
        }

        protected virtual HttpRequestMessage CreateRequest(GetEntityCommand cmd)
        {
            var req = new HttpRequest(HttpMethod.Get, GenerateRequestUrl(cmd.Id, cmd.Rev));

            req.SetIfMatch(cmd.Rev);

            return req;
        }

        protected virtual HttpRequestMessage CreateRequest(HttpMethod method, EntityCommand cmd)
        {
            var req = new HttpRequest(method, GenerateRequestUrl(cmd.Id, cmd.Rev));

            req.SetIfMatch(cmd.Rev);
            req.SetContent(cmd.Json);

            return req;
        }

        protected virtual string GenerateRequestUrl(string id, string rev)
        {
            return string.Format("{0}/{1}{2}",
                Client.Connection.Address,
                id ?? string.Empty,
                rev == null ? string.Empty : string.Concat("?rev=", rev));
        }

        protected virtual async Task<EntityResponse<T>> ProcessEntityResponseAsync<T>(Task<HttpResponseMessage> responseTask) where T : class
        {
            return Client.ResponseFactory.CreateEntityResponse<T>(await responseTask);
        }

        [Serializable]
        protected internal class EntityCommand
        {
            public string Id { get; set; }
            public string Rev { get; set; }
            public string Json { get; set; }
        }
    }
}