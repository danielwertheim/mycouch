using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories.Materializers
{
    public class ViewQueryResponseMaterializer
    {
        protected readonly IEntitySerializer EntitySerializer;

        public ViewQueryResponseMaterializer(IEntitySerializer entitySerializer)
        {
            Ensure.That(entitySerializer, "entitySerializer").IsNotNull();

            EntitySerializer = entitySerializer;
        }

        public virtual async void Materialize<TValue, TIncludedDoc>(ViewQueryResponse<TValue, TIncludedDoc> response, HttpResponseMessage httpResponse)
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                EntitySerializer.Populate(response, content);
            }
        }
    }
}