using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCouch.Extensions;
using System.Net.Http;
using MyCouch.HttpRequestFactories;

namespace MyCouch.Contexts
{
    public class Shows : ApiContextBase<IDbClientConnection>, IShows
    {
        protected ShowHttpRequestFactory ShowHttpRequestFactory { get; set; }
        protected RawResponseFactory RawResponseFactory { get; set; }
        public Shows(IDbClientConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            ShowHttpRequestFactory = new ShowHttpRequestFactory(serializer);
            RawResponseFactory = new RawResponseFactory(serializer);
        }

        public virtual async Task<RawResponse> QueryRawAsync(ShowRequest request)
        {
            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessRawHttpResponse(res);
            }
        }

        protected virtual HttpRequest CreateHttpRequest(ShowRequest request)
        {
            return ShowHttpRequestFactory.Create(request);
        }
        
        protected virtual RawResponse ProcessRawHttpResponse(HttpResponseMessage response)
        {
            return RawResponseFactory.Create(response);
        }
    }
}
