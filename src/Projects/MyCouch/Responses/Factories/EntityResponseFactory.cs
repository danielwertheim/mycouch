using System.Net.Http;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class EntityResponseFactory : ResponseFactoryBase
    {
        protected readonly ISerializer Serializer;

        public EntityResponseFactory(SerializationConfiguration serializationConfiguration, ISerializer serializer)
            : base(serializationConfiguration)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            Serializer = serializer;
        }

        public virtual EntityResponse<T> Create<T>(HttpResponseMessage response) where T : class
        {
            return BuildResponse(new EntityResponse<T>(), response, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse<T>(HttpResponseMessage response, EntityResponse<T> result) where T : class
        {
            using (var content = response.Content.ReadAsStream())
            {
                if (ContentShouldHaveIdAndRev(response.RequestMessage))
                    PopulateDocumentHeaderResponse(result, content);
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

        protected virtual void OnFailedResponse<T>(HttpResponseMessage response, EntityResponse<T> result) where T : class 
        {
            base.OnFailedResponse(response, result);

            AssignMissingIdFromRequestUri(response, result);
        }
    }
}