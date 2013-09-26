using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Databases : ApiContextBase, IDatabases
    {
        protected DatabaseResponseFactory DatabaseResponseFactory { get; set; }

        public Databases(IConnection connection, SerializationConfiguration serializationConfiguration) : base(connection)
        {
            Ensure.That(serializationConfiguration, "serializationConfiguration").IsNotNull();

            DatabaseResponseFactory = new DatabaseResponseFactory(serializationConfiguration);
        }

        public virtual async Task<DatabaseResponse> PutAsync()
        {
            using (var req = CreateRequest(HttpMethod.Put))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessResponse(res);
                }
            }
        }

        public virtual async Task<DatabaseResponse> DeleteAsync()
        {
            using (var req = CreateRequest(HttpMethod.Delete))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessResponse(res);
                }
            }
        }

        protected virtual HttpRequestMessage CreateRequest(HttpMethod method)
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