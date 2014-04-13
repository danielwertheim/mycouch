using System.Net.Http;
using EnsureThat;
using MyCouch.Responses.Factories.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class GetDatabaseResponseFactory : ResponseFactoryBase<GetDatabaseResponse>
    {
        protected readonly SimpleDeserializingResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public GetDatabaseResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new SimpleDeserializingResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        protected override GetDatabaseResponse CreateResponseInstance()
        {
            return new GetDatabaseResponse();
        }

        protected override void OnMaterializationOfSuccessfulResponseProperties(GetDatabaseResponse response, HttpResponseMessage httpResponse)
        {
            SuccessfulResponseMaterializer.Materialize(response, httpResponse);
        }

        protected override void OnMaterializationOfFailedResponseProperties(GetDatabaseResponse response, HttpResponseMessage httpResponse)
        {
            FailedResponseMaterializer.Materialize(response, httpResponse);
        }
    }
}