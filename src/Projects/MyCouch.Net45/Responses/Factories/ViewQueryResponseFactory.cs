using System.Net.Http;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class ViewQueryResponseFactory : ResponseFactoryBase
    {
        protected readonly IEntitySerializer EntitySerializer;

        public ViewQueryResponseFactory(ISerializer serializer, IEntitySerializer entitySerializer)
            : base(serializer)
        {
            Ensure.That(entitySerializer, "entitySerializer").IsNotNull();

            EntitySerializer = entitySerializer;
        }

        public virtual ViewQueryResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new ViewQueryResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        public virtual ViewQueryResponse<T> Create<T>(HttpResponseMessage httpResponse)
        {
            return Materialize(new ViewQueryResponse<T>(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        public virtual ViewQueryResponse<TValue, TIncludedDoc> Create<TValue, TIncludedDoc>(HttpResponseMessage httpResponse)
        {
            return Materialize(new ViewQueryResponse<TValue, TIncludedDoc>(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected async virtual void OnSuccessfulResponse<TValue, TIncludedDoc>(ViewQueryResponse<TValue, TIncludedDoc> response, HttpResponseMessage httpResponse)
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                EntitySerializer.Populate(response, content);
            }
        }
    }
}