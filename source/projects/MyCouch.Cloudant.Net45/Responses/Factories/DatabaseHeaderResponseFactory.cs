using System.Net.Http;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Responses.Factories.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class DatabaseHeaderResponseFactory : ResponseFactoryBase<DatabaseHeaderResponse>
    {
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public DatabaseHeaderResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        protected override DatabaseHeaderResponse CreateResponseInstance()
        {
            return new DatabaseHeaderResponse();
        }

        protected override void OnMaterializationOfSuccessfulResponseProperties(DatabaseHeaderResponse response, HttpResponseMessage httpResponse)
        {
            response.DbName = httpResponse.RequestMessage.RequestUri.ExtractDbName();
        }

        protected override void OnMaterializationOfFailedResponseProperties(DatabaseHeaderResponse response, HttpResponseMessage httpResponse)
        {
            FailedResponseMaterializer.Materialize(response, httpResponse);
        }
    }
}