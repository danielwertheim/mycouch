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

        public virtual EntityResponse<T> Create<T>(HttpResponseMessage httpResponse) where T : class
        {
            return Materialize(new EntityResponse<T>(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse<T>(EntityResponse<T> response, HttpResponseMessage httpResponse) where T : class
        {
            using (var content = httpResponse.Content.ReadAsStream())
            {
                if (ContentShouldHaveIdAndRev(httpResponse.RequestMessage))
                    AssignDocumentHeaderFromResponseStream(response, content);
                else
                {
                    AssignMissingIdFromRequestUri(response, httpResponse);
                    AssignMissingRevFromRequestHeaders(response, httpResponse);
                }

                if (response.RequestMethod == HttpMethod.Get)
                {
                    content.Position = 0;
                    response.Entity = Serializer.Deserialize<T>(content);
                }
            }
        }

        protected virtual void OnFailedResponse<T>(EntityResponse<T> response, HttpResponseMessage httpResponse) where T : class
        {
            base.OnFailedResponse(response, httpResponse);

            AssignMissingIdFromRequestUri(response, httpResponse);
        }
    }
}