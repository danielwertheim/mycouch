using System;
using System.Net.Http;
using MyCouch.Responses.Materializers;

namespace MyCouch.Responses.Factories
{
    public abstract class ResponseFactoryBase
    {
        protected readonly BasicResponseMaterializer BasicResponseMaterializer;

        protected ResponseFactoryBase()
        {
            BasicResponseMaterializer = new BasicResponseMaterializer();
        }

        protected virtual TResponse Materialize<TResponse>(
            HttpResponseMessage httpResponse,
            Action<TResponse, HttpResponseMessage> sucessfulResponseMaterializer,
            Action<TResponse, HttpResponseMessage> failedResponseMaterializer) where TResponse : Response, new()
        {
            var response = new TResponse();

            BasicResponseMaterializer.Materialize(response, httpResponse);

            if (response.IsSuccess)
                sucessfulResponseMaterializer(response, httpResponse);
            else
                failedResponseMaterializer(response, httpResponse);

            return response;
        }
    }
}