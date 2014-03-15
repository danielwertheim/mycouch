using System.IO;
using System.Net.Http;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories.Materializers
{
    public class EntityResponseMaterializer
    {
        protected readonly ISerializer Serializer;
        protected readonly IEntitySerializer EntitySerializer;

        public EntityResponseMaterializer(ISerializer serializer, IEntitySerializer entitySerializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();
            Ensure.That(entitySerializer, "entitySerializer").IsNotNull();

            Serializer = serializer;
            EntitySerializer = entitySerializer;
        }

        public virtual void Materialize<T>(EntityResponse<T> response, HttpResponseMessage httpResponse) where T : class
        {
            SetContent(response, httpResponse);
        }

        protected virtual async void SetContent<T>(EntityResponse<T> response, HttpResponseMessage httpResponse) where T : class
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                if (ContentShouldContainIdAndRev(httpResponse.RequestMessage))
                    SetDocumentHeaderFromResponseStream(response, content);
                else
                {
                    SetMissingIdFromRequestUri(response, httpResponse);
                    SetMissingRevFromRequestHeaders(response, httpResponse);
                }

                if (response.RequestMethod == HttpMethod.Get)
                {
                    content.Position = 0;
                    response.Content = EntitySerializer.Deserialize<T>(content);
                }
            }
        }

        protected virtual bool ContentShouldContainIdAndRev(HttpRequestMessage request)
        {
            return
                request.Method == HttpMethod.Post ||
                request.Method == HttpMethod.Put ||
                request.Method == HttpMethod.Delete;
        }

        protected virtual void SetDocumentHeaderFromResponseStream<T>(EntityResponse<T> response, Stream content) where T : class
        {
            Serializer.Populate(response, content);
        }

        protected virtual void SetMissingIdFromRequestUri<T>(EntityResponse<T> response, HttpResponseMessage httpResponse) where T : class
        {
            if (string.IsNullOrWhiteSpace(response.Id) && httpResponse.RequestMessage.Method != HttpMethod.Post)
                response.Id = httpResponse.RequestMessage.GetUriSegmentByRightOffset();
        }

        protected virtual void SetMissingRevFromRequestHeaders<T>(EntityResponse<T> response, HttpResponseMessage httpResponse) where T : class
        {
            if (string.IsNullOrWhiteSpace(response.Rev))
                response.Rev = httpResponse.Headers.GetETag();
        }
    }
}