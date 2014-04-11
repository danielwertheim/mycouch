using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories.Materializers
{
    public class ReplicationResponseMaterializer
    {
        protected readonly ISerializer Serializer;

        public ReplicationResponseMaterializer(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            Serializer = serializer;
        }

        public virtual async void Materialize(ReplicationResponse response, HttpResponseMessage httpResponse)
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                Serializer.Populate(response, content);
            }
        }
    }
}