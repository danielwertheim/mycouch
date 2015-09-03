using System.Net.Http;
using System.Text;
using MyCouch.EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class BulkHttpRequestFactory
    {
        public virtual HttpRequest Create(BulkRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            return new HttpRequest(HttpMethod.Post, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType())
                .SetJsonContent(GenerateRequestBody(request));
        }

        protected virtual string GenerateRelativeUrl(BulkRequest request)
        {
            return "/_bulk_docs";
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