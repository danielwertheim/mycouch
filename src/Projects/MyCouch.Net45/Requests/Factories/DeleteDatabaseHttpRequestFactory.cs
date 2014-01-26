using System.Net.Http;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class DeleteDatabaseHttpRequestFactory: DatabaseHttpRequestFactoryBase
    {
        public DeleteDatabaseHttpRequestFactory(IConnection connection) : base(connection) { }

        public virtual HttpRequest Create(DeleteDatabaseRequest request)
        {
            var httpRequest = CreateFor<DeleteDatabaseRequest>(HttpMethod.Delete, GenerateRequestUrl());

            return httpRequest;
        }
    }
}