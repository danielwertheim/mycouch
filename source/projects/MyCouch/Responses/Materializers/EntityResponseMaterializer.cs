using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.EntitySchemes;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Materializers
{
    public class EntityResponseMaterializer
    {
        protected readonly ISerializer Serializer;
        protected readonly IEntityReflector EntityReflector;

        public EntityResponseMaterializer(ISerializer serializer, IEntityReflector entityReflector)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));
            Ensure.Any.IsNotNull(entityReflector, nameof(entityReflector));

            Serializer = serializer;
            EntityReflector = entityReflector;
        }

        public virtual async Task MaterializeAsync<T>(EntityResponse<T> response, HttpResponseMessage httpResponse) where T : class
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                if (response is GetEntityResponse<T> get)
                {
                    response.Content = Serializer.Deserialize<T>(content);
                    response.Id = EntityReflector.IdMember.GetValueFrom(get.Content);
                    response.Rev = EntityReflector.RevMember.GetValueFrom(get.Content);
                }
                else
                    Serializer.Populate(response, content);

                SetMissingIdFromRequestUri(response, httpResponse.RequestMessage);
                SetMissingRevFromResponseHeaders(response, httpResponse.Headers);
            }
        }

        protected virtual void SetMissingIdFromRequestUri<T>(EntityResponse<T> response, HttpRequestMessage request) where T : class
        {
            if (string.IsNullOrWhiteSpace(response.Id) && request.Method != HttpMethod.Post)
                response.Id = request.ExtractIdFromUri(false);
        }

        protected virtual void SetMissingRevFromResponseHeaders<T>(EntityResponse<T> response, HttpResponseHeaders responseHeaders) where T : class
        {
            if (string.IsNullOrWhiteSpace(response.Rev))
                response.Rev = responseHeaders.GetETag();
        }
    }
}