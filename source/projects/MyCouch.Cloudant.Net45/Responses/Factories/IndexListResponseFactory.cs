using EnsureThat;
using MyCouch.Responses.Factories;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;
using System.Net.Http;

namespace MyCouch.Cloudant.Responses.Factories
{
    public class IndexListResponseFactory : ResponseFactoryBase<IndexListResponse>
    {
        protected readonly SimpleDeserializingResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;
        public IndexListResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new SimpleDeserializingResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        protected override void MaterializeSuccessfulResponse(IndexListResponse response, HttpResponseMessage httpResponse)
        {
            SuccessfulResponseMaterializer.Materialize(response, httpResponse);
        }

        protected override void MaterializeFailedResponse(IndexListResponse response, HttpResponseMessage httpResponse)
        {
            FailedResponseMaterializer.Materialize(response, httpResponse);
        }
    }
}
