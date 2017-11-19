using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class GetDatabaseHttpRequestFactory
    {
        public virtual HttpRequest Create(GetDatabaseRequest request)
        {
            Ensure.Any.IsNotNull(request, nameof(request));

            return new HttpRequest(HttpMethod.Get, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType());
        }

        protected virtual string GenerateRelativeUrl(GetDatabaseRequest request)
        {
            return "/";
        }
    }
}