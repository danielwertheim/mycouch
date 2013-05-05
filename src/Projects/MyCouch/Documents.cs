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

        public virtual BulkResponse Bulk(BulkCommand cmd)
        {
            return BulkAsync(cmd).Result;
        }

        public virtual async Task<BulkResponse> BulkAsync(BulkCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = SendAsync(req);

            return await ProcessHttpBulkResponseAsync(res);
        }

        public virtual JsonDocumentResponse Get(string id, string rev = null)
        {
            return GetAsync(id, rev).Result;
        }

        public virtual async Task<JsonDocumentResponse> GetAsync(string id, string rev = null)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();

            var req = CreateRequest(HttpMethod.Get, new DocumentCommand { Id = id, Rev = rev });
            var res = SendAsync(req);
            
            return await ProcessHttpJsonDocumentResponseAsync(res);
        }

        public virtual EntityResponse<T> Get<T>(string id, string rev = null) where T : class
        {
            return GetAsync<T>(id, rev).Result;
        }

        public virtual async Task<EntityResponse<T>> GetAsync<T>(string id, string rev = null) where T : class
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();

            var req = CreateRequest(HttpMethod.Get, new DocumentCommand { Id = id, Rev = rev });
            var res = SendAsync(req);
            
            return await ProcessHttpEntityResponseAsync<T>(res);
        }

        public virtual JsonDocumentResponse Post(string doc)
        {
            return PostAsync(doc).Result;
        }

        public virtual async Task<JsonDocumentResponse> PostAsync(string doc)
        {
            Ensure.That(doc, "entity").IsNotNullOrWhiteSpace();

            var req = CreateRequest(HttpMethod.Post, new DocumentCommand { Content = doc });
            var res = SendAsync(req);
            
            return await ProcessHttpJsonDocumentResponseAsync(res);
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
                new DocumentCommand
                {
                    Content = SerializeEntity(entity)
                });

            var res = SendAsync(req);
            var response = await ProcessHttpEntityResponseAsync<T>(res);
            response.Entity = entity;

            if (response.IsSuccess)
            {
                Client.EntityReflector.IdMember.SetValueTo(response.Entity, response.Id);
                Client.EntityReflector.RevMember.SetValueTo(response.Entity, response.Rev);
            }

            return response;
        }

        public virtual JsonDocumentResponse Put(string id, string doc)
        {
            return PutAsync(id, doc).Result;
        }

        public virtual async Task<JsonDocumentResponse> PutAsync(string id, string doc)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(doc, "entity").IsNotNullOrWhiteSpace();

            var req = CreateRequest(HttpMethod.Put, new DocumentCommand { Id = id, Content = doc });
            var res = SendAsync(req);
            
            return await ProcessHttpJsonDocumentResponseAsync(res);
        }

        public virtual JsonDocumentResponse Put(string id, string rev, string doc)
        {
            return PutAsync(id, rev, doc).Result;
        }

        public virtual async Task<JsonDocumentResponse> PutAsync(string id, string rev, string doc)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(doc, "entity").IsNotNullOrWhiteSpace();

            var req = CreateRequest(HttpMethod.Put, new DocumentCommand { Id = id, Rev = rev, Content = doc });
            var res = SendAsync(req);
            
            return await ProcessHttpJsonDocumentResponseAsync(res);
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
                new DocumentCommand
                {
                    Id = Client.EntityReflector.IdMember.GetValueFrom(entity),
                    Rev = Client.EntityReflector.RevMember.GetValueFrom(entity),
                    Content = SerializeEntity(entity)
                });
            var res = SendAsync(req);
            var response = await ProcessHttpEntityResponseAsync<T>(res);
            response.Entity = entity;

            if (response.IsSuccess)
                Client.EntityReflector.RevMember.SetValueTo(response.Entity, response.Rev);

            return response;
        }

        public virtual JsonDocumentResponse Delete(string id, string rev)
        {
            return DeleteAsync(id, rev).Result;
        }

        public virtual async Task<JsonDocumentResponse> DeleteAsync(string id, string rev)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(rev, "rev").IsNotNullOrWhiteSpace();

            var req = CreateRequest(HttpMethod.Delete, new DocumentCommand { Id = id, Rev = rev });
            var res = SendAsync(req);
            
            return await ProcessHttpJsonDocumentResponseAsync(res);
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
                new DocumentCommand
                {
                    Id = Client.EntityReflector.IdMember.GetValueFrom(entity),
                    Rev = Client.EntityReflector.RevMember.GetValueFrom(entity)
                });
            var res = SendAsync(req);
            var response = await ProcessHttpEntityResponseAsync<T>(res);
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

        protected virtual HttpRequestMessage CreateRequest(BulkCommand cmd)
        {
            var req = new HttpRequest(HttpMethod.Post, GenerateRequestUrl(cmd));

            req.SetContent(cmd.ToJson());

            return req;
        }

        protected virtual HttpRequestMessage CreateRequest(HttpMethod method, DocumentCommand cmd)
        {
            var req = new HttpRequest(method, GenerateRequestUrl(cmd));

            if (!string.IsNullOrWhiteSpace(cmd.Rev))
                req.SetIfMatch(cmd.Rev);

            req.SetContent(cmd.Content);

            return req;
        }

        protected virtual string GenerateRequestUrl(BulkCommand cmd)
        {
            return string.Format("{0}/_bulk_docs", Client.Connection.Address);
        }

        protected virtual string GenerateRequestUrl(DocumentCommand cmd)
        {
            return string.Format("{0}/{1}{2}",
                Client.Connection.Address,
                cmd.Id ?? string.Empty,
                cmd.Rev == null ? string.Empty : string.Concat("?rev=", cmd.Rev));
        }

        protected virtual async Task<BulkResponse> ProcessHttpBulkResponseAsync(Task<HttpResponseMessage> responseTask)
        {
            return Client.ResponseFactory.CreateBulkResponse(await responseTask);
        }

        protected virtual async Task<JsonDocumentResponse> ProcessHttpJsonDocumentResponseAsync(Task<HttpResponseMessage> responseTask)
        {
            return Client.ResponseFactory.CreateJsonDocumentResponse(await responseTask);
        }

        protected virtual async Task<EntityResponse<T>> ProcessHttpEntityResponseAsync<T>(Task<HttpResponseMessage> responseTask) where T : class
        {
            return Client.ResponseFactory.CreateEntityResponse<T>(await responseTask);
        }

        [Serializable]
        protected internal class DocumentCommand
        {
            public string Id { get; set; }
            public string Rev { get; set; }
            public string Content { get; set; }
        }
    }
}