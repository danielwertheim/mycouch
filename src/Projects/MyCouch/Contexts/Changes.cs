using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Requests.Factories;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Changes : ApiContextBase, IChanges
    {
        protected IHttpRequestFactory<GetChangesRequest> HttpRequestFactory { get; set; }
        protected ChangesResponseFactory ChangesResponseFactory { get; set; }

        public Changes(IConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            HttpRequestFactory = new GetChangesHttpRequestFactory(Connection);
            ChangesResponseFactory = new ChangesResponseFactory(serializer);
        }

        public virtual Task<ChangesResponse> GetAsync()
        {
            return GetAsync(new GetChangesRequest());
        }

        public virtual Task<ChangesResponse<TIncludedDoc>> GetAsync<TIncludedDoc>()
        {
            return GetAsync<TIncludedDoc>(new GetChangesRequest());
        }

        public virtual Task<ChangesResponse> GetAsync(ChangesFeed feed)
        {
            return GetAsync(new GetChangesRequest { Feed = feed });
        }

        public virtual Task<ChangesResponse<TIncludedDoc>> GetAsync<TIncludedDoc>(ChangesFeed feed)
        {
            return GetAsync<TIncludedDoc>(new GetChangesRequest { Feed = feed });
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

        public virtual async Task<ChangesResponse<TIncludedDoc>> GetAsync<TIncludedDoc>(GetChangesRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead).ForAwait())
                {
                    return ProcessHttpResponse<TIncludedDoc>(res);
                }
            }
        }

        protected virtual HttpRequest CreateHttpRequest(GetChangesRequest request)
        {
            return HttpRequestFactory.Create(request);
        }

        protected virtual ChangesResponse ProcessHttpResponse(HttpResponseMessage response)
        {
            return ChangesResponseFactory.Create(response);
        }

        protected virtual ChangesResponse<TIncludedDoc> ProcessHttpResponse<TIncludedDoc>(HttpResponseMessage response)
        {
            return ChangesResponseFactory.Create<TIncludedDoc>(response);
        }
    }
}