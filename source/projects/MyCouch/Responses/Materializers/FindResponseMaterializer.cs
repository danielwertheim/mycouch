using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyCouch.Responses.Materializers
{
    public class FindResponseMaterializer
    {
        protected readonly ISerializer Serializer;

        public FindResponseMaterializer(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            Serializer = serializer;
        }

        public virtual async Task MaterializeAsync<TIncludedDoc>(FindResponse<TIncludedDoc> response, HttpResponseMessage httpResponse)
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                Serializer.Populate(response, content);
            }
        }
    }
}
