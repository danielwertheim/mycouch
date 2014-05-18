using System.Net.Http;
using EnsureThat;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class ContinuousChangesResponseFactory : ResponseFactoryBase<ContinuousChangesResponse>
    {
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public ContinuousChangesResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        protected override void MaterializeSuccessfulResponse(ContinuousChangesResponse response, HttpResponseMessage httpResponse) { }

        protected override void MaterializeFailedResponse(ContinuousChangesResponse response, HttpResponseMessage httpResponse)
        {
            FailedResponseMaterializer.Materialize(response, httpResponse);
        }
    }
}