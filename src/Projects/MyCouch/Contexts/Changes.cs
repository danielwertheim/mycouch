using System;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Requests.Factories;
using MyCouch.Responses;

namespace MyCouch.Contexts
{
    public class Changes : ApiContextBase, IChanges
    {
        protected IHttpRequestFactory<GetChangesRequest> HttpRequestFactory { get; set; }

        public Changes(IConnection connection)
            : base(connection)
        {
            HttpRequestFactory = new GetChangesHttpRequestFactory(Connection);
        }

        public virtual Task<ChangesResponse> GetAsync()
        {
            return GetAsync(new GetChangesRequest());
        }

        public virtual Task<ChangesResponse> GetAsync(ChangesFeed feed)
        {
            return GetAsync(new GetChangesRequest { Feed = feed });
        }

        public virtual async Task<ChangesResponse> GetAsync(GetChangesRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead).ForAwait())
                {
                    return ProcessHttpResponse(res);
                }
            }
        }

        protected virtual HttpRequest CreateHttpRequest(GetChangesRequest request)
        {
            return HttpRequestFactory.Create(request);
        }

        protected virtual ChangesResponse ProcessHttpResponse(HttpResponseMessage response)
        {
            throw new NotImplementedException();
        }
    }
}