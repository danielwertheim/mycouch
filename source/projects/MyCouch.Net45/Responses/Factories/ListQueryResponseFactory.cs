using System.Net.Http;
using EnsureThat;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class ListQueryResponseFactory : ResponseFactoryBase
    {
        protected readonly ListQueryResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public ListQueryResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new ListQueryResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual ListQueryResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize<ListQueryResponse>(
                httpResponse,
                MaterializeSuccessfulResponse,
                MaterializeFailedResponse);
        }
        
        protected virtual void MaterializeSuccessfulResponse(ListQueryResponse response, HttpResponseMessage httpResponse)
        {
            SuccessfulResponseMaterializer.Materialize(response, httpResponse);
        }

        protected virtual void MaterializeFailedResponse(ListQueryResponse response, HttpResponseMessage httpResponse)
        {
            FailedResponseMaterializer.Materialize(response, httpResponse);
        }
    }
}