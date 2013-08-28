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
    public class Databases : IDatabases 
    {
        protected readonly IConnection Connection;
        protected readonly DatabaseResponseFactory DatabaseResponseFactory;

        public Databases(IConnection connection, SerializationConfiguration serializationConfiguration)
        {
            Ensure.That(connection, "connection").IsNotNull();
            Ensure.That(serializationConfiguration, "serializationConfiguration").IsNotNull();

            Connection = connection;
            DatabaseResponseFactory = new DatabaseResponseFactory(new DefaultResponseMaterializer(serializationConfiguration));
        }

        public virtual async Task<DatabaseResponse> PutAsync()
        {
            var req = CreateRequest(HttpMethod.Put);
            var res = SendAsync(req);

            return ProcessResponse(await res.ForAwait());
        }

        public virtual async Task<DatabaseResponse> DeleteAsync()
        {
            var req = CreateRequest(HttpMethod.Delete);
            var res = SendAsync(req);

            return ProcessResponse(await res.ForAwait());
        }

        protected virtual HttpRequestMessage CreateRequest(HttpMethod method)
        {
            return new HttpRequest(method, GenerateRequestUrl());
        }

        protected virtual string GenerateRequestUrl()
        {
            return Connection.Address.ToString();
        }

        protected virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return Connection.SendAsync(request);
        }

        protected virtual DatabaseResponse ProcessResponse(HttpResponseMessage response)
        {
            return DatabaseResponseFactory.Create(response);
        }
    }
}