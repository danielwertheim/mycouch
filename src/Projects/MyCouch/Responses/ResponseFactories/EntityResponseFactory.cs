using System.Net.Http;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.ResponseFactories
{
    public class EntityResponseFactory : DocumentHeaderResponseFactoryBase
    {
        protected readonly ISerializer Serializer;

        public EntityResponseFactory(IResponseMaterializer responseMaterializer, ISerializer serializer)
            : base(responseMaterializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            Serializer = serializer;
        }

        public virtual EntityResponse<T> Create<T>(HttpResponseMessage response) where T : class
        {
            return CreateResponse<EntityResponse<T>>(response, OnSuccessfulEntityResponseContentMaterializer, OnFailedEntityResponseContentMaterializer);
        }

        protected virtual void OnSuccessfulEntityResponseContentMaterializer<T>(HttpResponseMessage response, EntityResponse<T> result) where T : class
        {
            using (var content = response.Content.ReadAsStream())
            {
                if (ContentShouldHaveIdAndRev(response.RequestMessage))
                    ResponseMaterializer.PopulateDocumentHeaderResponse(result, content);
                else
                {
                    AssignMissingIdFromRequestUri(response, result);
                    AssignMissingRevFromRequestHeaders(response, result);
                }

                if (result.RequestMethod == HttpMethod.Get)
                {
                    content.Position = 0;
                    result.Entity = Serializer.Deserialize<T>(content);
                }
            }
        }

        protected virtual void OnFailedEntityResponseContentMaterializer<T>(HttpResponseMessage response, EntityResponse<T> result) where T : class 
        {
            OnFailedDocumentHeaderResponseContentMaterializer(response, result);
        }
    }
}