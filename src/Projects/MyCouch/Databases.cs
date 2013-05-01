using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch
{
    public class Databases : IDatabases 
    {
        protected readonly IClient Client;

        public Databases(IClient client)
        {
            Ensure.That(client, "Client").IsNotNull();

            Client = client;
        }

        public virtual DatabaseResponse Put(string dbname)
        {
            return PutAsync(dbname).Result;
        }

        public virtual async Task<DatabaseResponse> PutAsync(string dbname)
        {
            Ensure.That(dbname, "dbname").IsNotNullOrWhiteSpace();

            var req = CreateRequest(HttpMethod.Put, dbname);
            var res = SendAsync(req);

            return await ProcessHttpResponseAsync(res);
        }

        public virtual DatabaseResponse Delete(string dbname)
        {
            return DeleteAsync(dbname).Result;
        }

        public virtual async Task<DatabaseResponse> DeleteAsync(string dbname)
        {
            Ensure.That(dbname, "dbname").IsNotNullOrWhiteSpace();

            var req = CreateRequest(HttpMethod.Delete, dbname);
            var res = SendAsync(req);

            return await ProcessHttpResponseAsync(res);
        }

        protected virtual HttpRequestMessage CreateRequest(HttpMethod method, string dbname)
        {
            return new HttpRequest(method, GenerateRequestUrl(dbname));
        }

        protected virtual string GenerateRequestUrl(string dbname)
        {
            return string.Format("{0}/{1}",
                Client.Connection.Address.AbsoluteUri.Replace(Client.Connection.Address.PathAndQuery, string.Empty),
                dbname);
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