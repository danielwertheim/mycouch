using System.Net.Http;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;
using MyCouch.Net;

namespace MyCouch.Responses.Materializers
{
    public class ListQueryResponseMaterializer
    {
        protected readonly ISerializer Serializer;

        public ListQueryResponseMaterializer(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            Serializer = serializer;
        }

        public virtual async void Materialize(ListQueryResponse response, HttpResponseMessage httpResponse)
        {
            response.Etag = httpResponse.Headers.GetETag();
            response.Content = await httpResponse.Content.ReadAsStringAsync().ForAwait();
        }
    }
}