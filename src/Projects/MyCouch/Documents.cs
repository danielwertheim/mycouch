using System;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch
{
    public class Documents : IDocuments
    {
        protected readonly IClient Client;

        public Documents(IClient client)
        {
            Ensure.That(client, "Client").IsNotNull();

            Client = client;
        }

        public virtual DocumentResponse Get(string id, string rev = null)
        {
            return GetAsync(id, rev).Result;
        }

        public virtual async Task<DocumentResponse> GetAsync(string id, string rev = null)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();

            var req = CreateRequest(HttpMethod.Get, new DocCommand { Id = id, Rev = rev });
            var res = SendAsync(req);
            
            return await ProcessHttpResponseAsync(res);
        }

        public virtual EntityResponse<T> Get<T>(string id, string rev = null) where T : class
        {
            return GetAsync<T>(id, rev).Result;
        }

        public virtual async Task<EntityResponse<T>> GetAsync<T>(string id, string rev = null) where T : class
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();

            var req = CreateRequest(HttpMethod.Get, new DocCommand { Id = id, Rev = rev });
            var res = SendAsync(req);
            
            return await ProcessHttpResponseAsync<T>(res);
        }

        public virtual DocumentResponse Post(string doc)
        {
            return PostAsync(doc).Result;
        }

        public virtual async Task<DocumentResponse> PostAsync(string doc)
        {
            Ensure.That(doc, "entity").IsNotNullOrWhiteSpace();

            var req = CreateRequest(HttpMethod.Post, new DocCommand { Content = doc });
            var res = SendAsync(req);
            
            return await ProcessHttpResponseAsync(res);
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
                new DocCommand
                {
                    Content = SerializeEntity(entity)
                });

            var res = SendAsync(req);
            var response = await ProcessHttpResponseAsync<T>(res);
            response.Entity = entity;

            if (response.IsSuccess)
            {
                Client.EntityReflector.IdMember.SetValueTo(response.Entity, response.Id);
                Client.EntityReflector.RevMember.SetValueTo(response.Entity, response.Rev);
            }

            return response;
        }

        public virtual DocumentResponse Put(string id, string doc)
        {
            return PutAsync(id, doc).Result;
        }

        public virtual async Task<DocumentResponse> PutAsync(string id, string doc)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(doc, "entity").IsNotNullOrWhiteSpace();

            var req = CreateRequest(HttpMethod.Put, new DocCommand { Id = id, Content = doc });
            var res = SendAsync(req);
            
            return await ProcessHttpResponseAsync(res);
        }

        public virtual DocumentResponse Put(string id, string rev, string doc)
        {
            return PutAsync(id, rev, doc).Result;
        }

        public virtual async Task<DocumentResponse> PutAsync(string id, string rev, string doc)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(doc, "entity").IsNotNullOrWhiteSpace();

            var req = CreateRequest(HttpMethod.Put, new DocCommand { Id = id, Rev = rev, Content = doc });
            var res = SendAsync(req);
            
            return await ProcessHttpResponseAsync(res);
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
                new DocCommand
                {
                    Id = Client.EntityReflector.IdMember.GetValueFrom(entity),
                    Rev = Client.EntityReflector.RevMember.GetValueFrom(entity),
                    Content = SerializeEntity(entity)
                });
            var res = SendAsync(req);
            var response = await ProcessHttpResponseAsync<T>(res);
            response.Entity = entity;

            if (response.IsSuccess)
                Client.EntityReflector.RevMember.SetValueTo(response.Entity, response.Rev);

            return response;
        }

        public virtual DocumentResponse Delete(string id, string rev)
        {
            return DeleteAsync(id, rev).Result;
        }

        public virtual async Task<DocumentResponse> DeleteAsync(string id, string rev)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(rev, "rev").IsNotNullOrWhiteSpace();

            var req = CreateRequest(HttpMethod.Delete, new DocCommand { Id = id, Rev = rev });
            var res = SendAsync(req);
            
            return await ProcessHttpResponseAsync(res);
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
                new DocCommand
                {
                    Id = Client.EntityReflector.IdMember.GetValueFrom(entity),
                    Rev = Client.EntityReflector.RevMember.GetValueFrom(entity)
                });
            var res = SendAsync(req);
            var response = await ProcessHttpResponseAsync<T>(res);
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

        protected virtual HttpRequestMessage CreateRequest(HttpMethod method, DocCommand cmd)
        {
            var req = new HttpRequest(method, GenerateRequestUrl(cmd));

            if(!string.IsNullOrWhiteSpace(cmd.Rev))
                req.SetIfMatch(cmd.Rev);
            
            req.SetContent(cmd.Content);

            return req;
        }

        protected virtual string GenerateRequestUrl(DocCommand cmd)
        {
            return string.Format("{0}/{1}{2}",
                Client.Connection.Address,
                cmd.Id ?? string.Empty,
                cmd.Rev == null ? string.Empty : string.Concat("?rev=", cmd.Rev));
        }

        protected virtual async Task<DocumentResponse> ProcessHttpResponseAsync(Task<HttpResponseMessage> responseTask)
        {
            return Client.ResponseFactory.CreateDocumentResponse(await responseTask);
        }

        protected virtual async Task<EntityResponse<T>> ProcessHttpResponseAsync<T>(Task<HttpResponseMessage> responseTask) where T : class
        {
            return Client.ResponseFactory.CreateEntityResponse<T>(await responseTask);
        }

        [Serializable]
        protected internal class DocCommand
        {
            public string Id { get; set; }
            public string Rev { get; set; }
            public string Content { get; set; }
        }
    }
}