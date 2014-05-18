using System.Net.Http;
using EnsureThat;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class TextResponseFactory : ResponseFactoryBase<TextResponse>
    {
        protected readonly TextResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public TextResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new TextResponseMaterializer();
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        protected override void MaterializeSuccessfulResponse(TextResponse response, HttpResponseMessage httpResponse)
        {
            SuccessfulResponseMaterializer.Materialize(response, httpResponse);
        }

        protected override void MaterializeFailedResponse(TextResponse response, HttpResponseMessage httpResponse)
        {
            FailedResponseMaterializer.Materialize(response, httpResponse);
        }
    }
}