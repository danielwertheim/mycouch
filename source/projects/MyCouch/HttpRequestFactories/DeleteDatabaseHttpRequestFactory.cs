using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class DeleteDatabaseHttpRequestFactory
    {
        public virtual HttpRequest Create(DeleteDatabaseRequest request)
        {
            Ensure.Any.IsNotNull(request, nameof(request));

            return new HttpRequest(HttpMethod.Delete, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType());
        }

        protected virtual string GenerateRelativeUrl(DeleteDatabaseRequest request)
        {
            return "/";
        }
    }
}