using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Materializers
{
    public class SimpleDeserializingResponseMaterializer
    {
        protected ISerializer Serializer { get; private set; }

        public SimpleDeserializingResponseMaterializer(ISerializer serializer)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            Serializer = serializer;
        }

        public virtual async Task MaterializeAsync<TResponse>(TResponse response, HttpResponseMessage httpResponse) where TResponse : Response
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                Serializer.Populate(response, content);
            }
        }
    }
}