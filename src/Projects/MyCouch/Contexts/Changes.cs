using System;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Responses;

namespace MyCouch.Contexts
{
    public class Changes : ApiContextBase, IChanges
    {
        public Changes(IConnection connection) : base(connection) {}

        public virtual Task<ChangesResponse> GetAsync(ChangesFeed feed)
        {
            return GetAsync(new GetChangesRequest(feed));
        }

        public virtual async Task<ChangesResponse> GetAsync(GetChangesRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessHttpResponse(res);
                }
            }
        }

        protected virtual HttpRequest CreateHttpRequest(GetChangesRequest request)
        {
            throw new NotImplementedException();
        }

        protected virtual ChangesResponse ProcessHttpResponse(HttpResponseMessage response)
        {
            throw new NotImplementedException();
        }
    }
}