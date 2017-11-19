using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Materializers
{
    public class ViewQueryResponseMaterializer
    {
        protected readonly ISerializer Serializer;

        public ViewQueryResponseMaterializer(ISerializer serializer)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

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