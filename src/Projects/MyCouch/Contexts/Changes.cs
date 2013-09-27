using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Commands;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Responses;

namespace MyCouch.Contexts
{
    public class Changes : ApiContextBase, IChanges
    {
        public Changes(IConnection connection) : base(connection) { }

        public virtual Task<ChangesResponse> GetAsync(ChangesFeed feed)
        {
            return GetAsync(new GetChangesCommand(feed));
        }

        public virtual async Task<ChangesResponse> GetAsync(GetChangesCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            using (var req = CreateRequest(cmd))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessHttpResponse(res);
                }
            }
        }

        protected virtual HttpRequestMessage CreateRequest(GetChangesCommand cmd)
        {
            return new HttpRequest(HttpMethod.Get, GenerateRequestUrl(cmd));
        }

        protected virtual string GenerateRequestUrl(GetChangesCommand cmd)
        {
            var querystring = "";

            return string.Format("{0}/_changes?{1}",
                Connection.Address,
                querystring);
        }

        protected virtual ChangesResponse ProcessHttpResponse(HttpResponseMessage response)
        {
            return null;
        }
    }
}