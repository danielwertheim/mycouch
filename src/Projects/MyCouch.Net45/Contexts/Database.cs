using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Requests.Factories;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Database : ApiContextBase, IDatabase
    {
        protected DatabaseResponseFactory DatabaseResponseFactory { get; set; }
        protected CompactDatabaseHttpRequestFactory CompactDatabaseHttpRequestFactory { get; set; }

        public Database(IConnection connection, ISerializer serializer) : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            DatabaseResponseFactory = new DatabaseResponseFactory(serializer);
            CompactDatabaseHttpRequestFactory = new CompactDatabaseHttpRequestFactory(Connection);
        }

        public virtual async Task<DatabaseResponse> PutAsync()
        {
            using (var req = CreateHttpRequest(HttpMethod.Put))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessResponse(res);
                }
            }
        }

        public virtual async Task<DatabaseResponse> DeleteAsync()
        {
            using (var req = CreateHttpRequest(HttpMethod.Delete))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessResponse(res);
                }
            }
        }

        public virtual async Task<DatabaseResponse> CompactAsync()
        {
            using (var req = CreateHttpRequest(new CompactDatabaseRequest()))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessResponse(res);
                }
            }
        }

        protected virtual HttpRequest CreateHttpRequest(CompactDatabaseRequest request)
        {
            return CompactDatabaseHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest(HttpMethod method)
        {
            return new HttpRequest(method, GenerateRequestUrl());
        }

        protected virtual string GenerateRequestUrl()
        {
            return Connection.Address.ToString();
        }

        protected virtual DatabaseResponse ProcessResponse(HttpResponseMessage response)
        {
            return DatabaseResponseFactory.Create(response);
        }
    }
}