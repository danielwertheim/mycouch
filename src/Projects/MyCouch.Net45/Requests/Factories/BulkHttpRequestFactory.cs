using System.Net.Http;
using System.Text;
using MyCouch.EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class BulkHttpRequestFactory : HttpRequestFactoryBase
    {
        protected ConstantRequestUrlGenerator RequestUrlGenerator { get; private set; }

        public BulkHttpRequestFactory(IDbClientConnection connection)
        {
            Ensure.That(connection, "connection").IsNotNull();

            RequestUrlGenerator = new ConstantRequestUrlGenerator(connection.Address, connection.DbName);
        }

        public virtual HttpRequest Create(BulkRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateFor<BulkRequest>(HttpMethod.Post, GenerateRequestUrl(request));

            httpRequest.SetJsonContent(GenerateRequestBody(request));

            return httpRequest;
        }

        protected virtual string GenerateRequestUrl(BulkRequest request)
        {
            return string.Format("{0}/_bulk_docs", RequestUrlGenerator.Generate());
        }

        protected virtual string GenerateRequestBody(BulkRequest request)
        {
            var sb = new StringBuilder();
            var documents = request.GetDocuments();

            sb.Append("{\"docs\":[");
            for (var i = 0; i < documents.Length; i++)
            {
                sb.Append(documents[i]);
                if (i < documents.Length - 1)
                    sb.Append(",");
            }
            sb.Append("]}");

            return sb.ToString();
        }
    }
}