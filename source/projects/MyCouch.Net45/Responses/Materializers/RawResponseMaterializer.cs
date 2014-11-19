using System.Net.Http;
using MyCouch.Extensions;

namespace MyCouch.Responses.Materializers
{
    public class RawResponseMaterializer
    {
        public virtual void Materialize(RawResponse response, HttpResponseMessage httpResponse)
        {
            SetContent(response, httpResponse);
        }

        protected virtual async void SetContent(RawResponse response, HttpResponseMessage httpResponse)
        {
            response.Content = await httpResponse.Content.ReadAsStringAsync().ForAwait();
        }
    }
}