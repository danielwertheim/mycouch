using System.Net.Http;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Responses.Factories
{
    public class SearchIndexResponseFactory : ResponseFactoryBase
    {
        protected readonly IEntitySerializer EntitySerializer;

        public SearchIndexResponseFactory(ISerializer serializer, IEntitySerializer entitySerializer)
            : base(serializer)
        {
            Ensure.That(entitySerializer, "entitySerializer").IsNotNull();

            EntitySerializer = entitySerializer;
        }

        public virtual SearchIndexResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new SearchIndexResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        public virtual SearchIndexResponse<TIncludedDoc> Create<TIncludedDoc>(HttpResponseMessage httpResponse)
        {
            return Materialize(new SearchIndexResponse<TIncludedDoc>(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected async virtual void OnSuccessfulResponse<TIncludedDoc>(SearchIndexResponse<TIncludedDoc> response, HttpResponseMessage httpResponse)
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                EntitySerializer.Populate(response, content);
            }
        }
    }
}