using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories.Materializers
{
    public class BulkResponseMaterializer
    {
        protected readonly ISerializer Serializer;

        public BulkResponseMaterializer(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            Serializer = serializer;
        }

        public virtual void Materialize(BulkResponse response, HttpResponseMessage httpResponse)
        {
            SetRows(response, httpResponse);
        }

        protected virtual async void SetRows(BulkResponse response, HttpResponseMessage httpResponse)
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
                response.Rows = Serializer.Deserialize<BulkResponse.Row[]>(content);
        }
    }
}