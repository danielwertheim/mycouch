using EnsureThat;
using MyCouch.Cloudant.Requests;
using MyCouch.Net;
using MyCouch.Serialization;
using System.Net.Http;

namespace MyCouch.Cloudant.HttpRequestFactories
{
    public class DeleteIndexHttpRequestFactory
    {
        private const string UrlFormat = "/_index/{0}/{1}/{2}";
        protected ISerializer Serializer { get; private set; }

        public virtual HttpRequest Create(DeleteIndexRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            return new HttpRequest(HttpMethod.Delete, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType());
        }

        protected virtual string GenerateRelativeUrl(DeleteIndexRequest request)
        {
            return string.Format(UrlFormat, request.DesignDoc, request.Type.AsString(), request.Name);
        }
    }
}
