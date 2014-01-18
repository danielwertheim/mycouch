using System.Net.Http;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class EntityResponseFactory : ResponseFactoryBase
    {
        protected readonly ISerializer EntitySerializer;

        public EntityResponseFactory(ISerializer serializer, ISerializer entitySerializer)
            : base(serializer)
        {
            Ensure.That(entitySerializer, "entitySerializer").IsNotNull();

            EntitySerializer = entitySerializer;
        }

        public virtual EntityResponse<T> Create<T>(HttpResponseMessage httpResponse) where T : class
        {
            return Materialize(new EntityResponse<T>(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected async virtual void OnSuccessfulResponse<T>(EntityResponse<T> response, HttpResponseMessage httpResponse) where T : class
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                if (ContentShouldHaveIdAndRev(httpResponse.RequestMessage))
                    PopulateDocumentHeaderFromResponseStream(response, content);
                else
                {
                    PopulateMissingIdFromRequestUri(response, httpResponse);
                    PopulateMissingRevFromRequestHeaders(response, httpResponse);
                }

                if (response.RequestMethod == HttpMethod.Get)
                {
                    content.Position = 0;
                    response.Entity = EntitySerializer.Deserialize<T>(content);
                }
            }
        }

        protected virtual void OnFailedResponse<T>(EntityResponse<T> response, HttpResponseMessage httpResponse) where T : class
        {
            base.OnFailedResponse(response, httpResponse);

            PopulateMissingIdFromRequestUri(response, httpResponse);
        }
    }
}