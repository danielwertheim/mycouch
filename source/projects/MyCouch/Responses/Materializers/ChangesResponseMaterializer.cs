using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Materializers
{
    public class ChangesResponseMaterializer
    {
        protected readonly ISerializer Serializer;

        public ChangesResponseMaterializer(ISerializer serializer)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            Serializer = serializer;
        }

        public virtual async Task MaterializeAsync<T>(ChangesResponse<T> response, HttpResponseMessage httpResponse)
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
                Serializer.Populate(response, content);
        }
    }
}