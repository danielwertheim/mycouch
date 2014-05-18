using System.Net.Http;
using MyCouch.Extensions;

namespace MyCouch.Responses.Materializers
{
    public class TextResponseMaterializer
    {
        public virtual void Materialize(TextResponse response, HttpResponseMessage httpResponse)
        {
            SetContent(response, httpResponse);
        }

        protected virtual async void SetContent(TextResponse response, HttpResponseMessage httpResponse)
        {
            response.Content = await httpResponse.Content.ReadAsStringAsync().ForAwait();
        }
    }
}