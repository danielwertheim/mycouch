using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;
using System.Net.Http;

namespace MyCouch.Cloudant.Responses.Materializers
{
    public class FindResponseMaterializer
    {
        protected readonly ISerializer Serializer;

        public FindResponseMaterializer(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            Serializer = serializer;
        }

        public virtual async void Materialize<TIncludedDoc>(FindResponse<TIncludedDoc> response, HttpResponseMessage httpResponse)
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                Serializer.Populate(response, content);
            }
        }
    }
}
