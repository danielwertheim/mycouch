using System.Net.Http;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Responses.Materializers
{
    public class SearchIndexResponseMaterializer
    {
        protected readonly ISerializer Serializer;

        public SearchIndexResponseMaterializer(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            Serializer = serializer;
        }

        public virtual async void Materialize<TIncludedDoc>(SearchIndexResponse<TIncludedDoc> response, HttpResponseMessage httpResponse)
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                Serializer.Populate(response, content);
            }
        }
    }
}