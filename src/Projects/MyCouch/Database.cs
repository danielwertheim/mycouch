using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch
{
    public class Database : IDatabase 
    {
        protected readonly IClient Client;

        public Database(IClient client)
        {
            Ensure.That(client, "Client").IsNotNull();

            Client = client;
        }

        public virtual DatabaseResponse Put()
        {
            return PutAsync().Result;
        }

        public virtual async Task<DatabaseResponse> PutAsync()
        {
            var req = CreateRequest(HttpMethod.Put);
            var res = SendAsync(req);

            return await ProcessHttpResponseAsync(res);
        }

        public virtual DatabaseResponse Delete()
        {
            return DeleteAsync().Result;
        }

        public virtual async Task<DatabaseResponse> DeleteAsync()
        {
            var req = CreateRequest(HttpMethod.Delete);
            var res = SendAsync(req);

            return await ProcessHttpResponseAsync(res);
        }

        protected virtual HttpRequestMessage CreateRequest(HttpMethod method)
        {
            return new HttpRequest(method, GenerateRequestUrl());
        }

        protected virtual string GenerateRequestUrl()
        {
            return Client.Connection.Address.ToString();
        }

        protected virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return Client.Connection.SendAsync(request);
        }

        protected virtual async Task<DatabaseResponse> ProcessHttpResponseAsync(Task<HttpResponseMessage> responseTask)
        {
            return Client.ResponseFactory.CreateDatabaseResponse(await responseTask);
        }
    }
}