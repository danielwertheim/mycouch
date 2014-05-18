using System.Net.Http;
using EnsureThat;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class ViewQueryResponseFactory : ResponseFactoryBase
    {
        protected readonly ViewQueryResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public ViewQueryResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new ViewQueryResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual ViewQueryResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize<ViewQueryResponse>(
                httpResponse,
                MaterializeSuccessfulResponse,
                MaterializeFailedResponse);
        }

        public virtual ViewQueryResponse<TValue> Create<TValue>(HttpResponseMessage httpResponse)
        {
            return Materialize<ViewQueryResponse<TValue>>(
                httpResponse,
                MaterializeSuccessfulResponse,
                MaterializeFailedResponse);
        }

        public virtual ViewQueryResponse<TValue, TIncludedDoc> Create<TValue, TIncludedDoc>(HttpResponseMessage httpResponse)
        {
            return Materialize<ViewQueryResponse<TValue, TIncludedDoc>>(
                httpResponse,
                MaterializeSuccessfulResponse,
                MaterializeFailedResponse);
        }

        protected virtual void MaterializeSuccessfulResponse<TValue, TIncludedDoc>(ViewQueryResponse<TValue, TIncludedDoc> response, HttpResponseMessage httpResponse)
        {
            SuccessfulResponseMaterializer.Materialize(response, httpResponse);
        }

        protected virtual void MaterializeFailedResponse<TValue, TIncludedDoc>(ViewQueryResponse<TValue, TIncludedDoc> response, HttpResponseMessage httpResponse)
        {
            FailedResponseMaterializer.Materialize(response, httpResponse);
        }
    }
}