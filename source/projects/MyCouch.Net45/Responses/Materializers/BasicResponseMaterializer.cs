using System.Net.Http;

namespace MyCouch.Responses.Materializers
{
    public class BasicResponseMaterializer
    {
        public virtual void Materialize(Response response, HttpResponseMessage httpResponse)
        {
            response.RequestUri = httpResponse.RequestMessage.RequestUri;
            response.StatusCode = httpResponse.StatusCode;
            response.RequestMethod = httpResponse.RequestMessage.Method;
            response.ContentLength = httpResponse.Content.Headers.ContentLength;
            response.ContentType = httpResponse.Content.Headers.ContentType.ToString();
        }
    }
}