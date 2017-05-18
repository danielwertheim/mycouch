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
            Ensure.That(serializer, "serializer").IsNotNull();
            Ensure.That(entityReflector, "entityReflector").IsNotNull();

            Serializer = serializer;
            EntityReflector = entityReflector;
        }

        public virtual async Task MaterializeAsync<T>(EntityResponse<T> response, HttpResponseMessage httpResponse) where T : class
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                var get = response as GetEntityResponse<T>;
                if (get != null)
                {
                    response.Content = Serializer.Deserialize<T>(content);
                    response.Id = EntityReflector.IdMember.GetValueFrom(response.Content);
                    response.Rev = EntityReflector.RevMember.GetValueFrom(response.Content);
                }
                else
                    Serializer.Populate(response, content);

                SetMissingIdFromRequestUri(response, httpResponse.RequestMessage);
                SetMissingRevFromRequestHeaders(response, httpResponse.Headers);
            }
        }

        protected virtual void SetMissingIdFromRequestUri<T>(EntityResponse<T> response, HttpRequestMessage request) where T : class
        {
            if (string.IsNullOrWhiteSpace(response.Id) && request.Method != HttpMethod.Post)
                response.Id = request.ExtractIdFromUri(false);
        }

        protected virtual void SetMissingRevFromRequestHeaders<T>(EntityResponse<T> response, HttpResponseHeaders responseHeaders) where T : class
        {
            if (string.IsNullOrWhiteSpace(response.Rev))
                response.Rev = responseHeaders.GetETag();
        }
    }
}