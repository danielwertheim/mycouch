using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Materializers
{
    public class BulkResponseMaterializer
    {
        protected readonly ISerializer Serializer;

        public BulkResponseMaterializer(ISerializer serializer)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            Serializer = serializer;
        }

        public virtual async Task MaterializeAsync(BulkResponse response, HttpResponseMessage httpResponse)
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
                response.Rows = Serializer.Deserialize<BulkResponse.Row[]>(content);
        }
    }
}