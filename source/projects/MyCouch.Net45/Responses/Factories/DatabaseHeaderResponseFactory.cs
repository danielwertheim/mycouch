using System.Net.Http;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Responses.Materializers;
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

        protected override void MaterializeSuccessfulResponse(DatabaseHeaderResponse response, HttpResponseMessage httpResponse)
        {
            response.DbName = httpResponse.RequestMessage.RequestUri.ExtractDbName();
        }

        protected override void MaterializeFailedResponse(DatabaseHeaderResponse response, HttpResponseMessage httpResponse)
        {
            FailedResponseMaterializer.Materialize(response, httpResponse);
        }
    }
}