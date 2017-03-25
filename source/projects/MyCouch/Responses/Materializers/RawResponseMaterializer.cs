using System.Net.Http;
using System.Threading.Tasks;
using MyCouch.Extensions;

namespace MyCouch.Responses.Materializers
{
    public class RawResponseMaterializer
    {
        public virtual async Task MaterializeAsync(RawResponse response, HttpResponseMessage httpResponse)
        {
            response.Content = await httpResponse.Content.ReadAsStringAsync().ForAwait();
        }
    }
}