using System.Net.Http;
using System.Threading.Tasks;
using MyCouch.Extensions;

namespace MyCouch.Responses.Materializers
{
    public class BasicResponseMaterializer
    {
        public virtual Task MaterializeAsync(Response response, HttpResponseMessage httpResponse)
        {
            response.RequestUri = httpResponse.RequestMessage.RequestUri;
            response.StatusCode = httpResponse.StatusCode;
            response.RequestMethod = httpResponse.RequestMessage.Method;
            response.ContentLength = httpResponse.Content.Headers.ContentLength;
            response.ContentType = httpResponse.Content.Headers.ContentType?.ToString();
            response.ETag = httpResponse.Headers.GetETag();

            return Task.FromResult(true);
        }
    }
}