using System.Net.Http;
using System.Threading.Tasks;
using MyCouch.EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Materializers
{
    public class ViewQueryResponseMaterializer
    {
        protected readonly ISerializer Serializer;

        public ViewQueryResponseMaterializer(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            Serializer = serializer;
        }

        public virtual async Task MaterializeAsync<TValue, TIncludedDoc>(ViewQueryResponse<TValue, TIncludedDoc> response, HttpResponseMessage httpResponse)
        {
            response.ETag = httpResponse.Headers.GetETag();

            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
                Serializer.Populate(response, content);
        }
    }
}