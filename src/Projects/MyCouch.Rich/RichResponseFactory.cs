using System.Net.Http;
using MyCouch.Net;
using MyCouch.Rich.Serialization;

namespace MyCouch.Rich
{
    public class RichResponseFactory : ResponseFactory, IRichResponseFactory
    {
        public RichResponseFactory(IRichSerializer serializer) : base(serializer) { }

        public virtual EntityResponse<T> CreateEntityResponse<T>(HttpResponseMessage response) where T : class
        {
            return CreateResponse<EntityResponse<T>>(response, OnSuccessfulEntityResponseContentMaterializer, OnFailedEntityResponseContentMaterializer);
        }

        protected virtual void OnSuccessfulEntityResponseContentMaterializer<T>(HttpResponseMessage response, EntityResponse<T> result) where T : class
        {
            using (var content = response.Content.ReadAsStream())
            {
                if (ContentShouldHaveIdAndRev(response.RequestMessage))
                    Serializer.PopulateDocumentHeaderResponse(result, content);
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