using System;
using System.Net.Http;
using EnsureThat;
using MyCouch.Responses.Materializers;

namespace MyCouch.Responses.Factories
{
    public abstract class ResponseFactoryBase<TResponse> : ResponseFactoryBase where TResponse : Response, new()
    {
        public virtual TResponse Create(HttpResponseMessage httpResponse)
        {
            Ensure.That(httpResponse, "httpResponse").IsNotNull();

            return Materialize<TResponse>(
                httpResponse,
                MaterializeSuccessfulResponse,
                MaterializeFailedResponse);
        }

        protected abstract void MaterializeSuccessfulResponse(TResponse response, HttpResponseMessage httpResponse);

        protected abstract void MaterializeFailedResponse(TResponse response, HttpResponseMessage httpResponse);
    }

    public abstract class ResponseFactoryBase
    {
        protected readonly BasicResponseMaterializer BasicResponseMaterializer;

        protected ResponseFactoryBase()
        {
            BasicResponseMaterializer = new BasicResponseMaterializer();
        }

        protected virtual TResponse Materialize<TResponse>(
            HttpResponseMessage httpResponse,
            Action<TResponse, HttpResponseMessage> materializeSuccessfulResponse,
            Action<TResponse, HttpResponseMessage> materializeFailedResponse) where TResponse : Response, new()
        {
            var response = new TResponse();

            MaterializeBasicResponseProperties(response, httpResponse);

            if (response.IsSuccess)
                materializeSuccessfulResponse(response, httpResponse);
            else
                materializeFailedResponse(response, httpResponse);

            return response;
        }

        protected virtual void MaterializeBasicResponseProperties<TResponse>(TResponse response, HttpResponseMessage httpResponse) where TResponse : Response
        {
            BasicResponseMaterializer.Materialize(response, httpResponse);
        }
    }
}