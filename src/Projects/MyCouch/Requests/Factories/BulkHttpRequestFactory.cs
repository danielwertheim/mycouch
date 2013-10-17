using System.Net.Http;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class BulkHttpRequestFactory : HttpRequestFactoryBase
    {
        public BulkHttpRequestFactory(IConnection connection) : base(connection) { }

        public virtual HttpRequest Create(BulkRequest request)
        {
            var createHttpRequest = CreateFor<BulkRequest>(HttpMethod.Post, GenerateRequestUrl(request));

            createHttpRequest.SetContent(request.ToJson());

            return createHttpRequest;
        }

        protected virtual string GenerateRequestUrl(BulkRequest request)
        {
            return string.Format("{0}/_bulk_docs", Connection.Address);
        }
    }
}