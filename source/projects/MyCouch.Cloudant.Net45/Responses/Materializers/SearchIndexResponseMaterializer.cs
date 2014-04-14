using System.Net.Http;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Responses.Materializers
{
    public class SearchIndexResponseMaterializer
    {
        protected readonly IEntitySerializer EntitySerializer;

        public SearchIndexResponseMaterializer(IEntitySerializer entitySerializer)
        {
            Ensure.That(entitySerializer, "entitySerializer").IsNotNull();

            EntitySerializer = entitySerializer;
        }

        public virtual async void Materialize<TIncludedDoc>(SearchIndexResponse<TIncludedDoc> response, HttpResponseMessage httpResponse)
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                EntitySerializer.Populate(response, content);
            }
        }
    }
}