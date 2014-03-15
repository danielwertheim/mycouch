using System;
using System.Net.Http;
using EnsureThat;
using MyCouch.Responses.Factories.Materializers;

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
            TResponse response,
            HttpResponseMessage httpResponse,
            Action<TResponse, HttpResponseMessage> onMaterializationOfSuccessfulResponseProperties,
            Action<TResponse, HttpResponseMessage> onMaterializationOfFailedResponseProperties) where TResponse : Response
        {
            OnMaterializationOfBasicResponseProperties(response, httpResponse);

            if (response.IsSuccess)
                onMaterializationOfSuccessfulResponseProperties(response, httpResponse);
            else
                onMaterializationOfFailedResponseProperties(response, httpResponse);

            return response;
        }

        protected virtual void OnMaterializationOfBasicResponseProperties<TResponse>(TResponse response, HttpResponseMessage httpResponse) where TResponse : Response
        {
            BasicResponseMaterializer.Materialize(response, httpResponse);
        }
    }

    public abstract class ResponseFactoryBase<T> : ResponseFactoryBase where T : Response
    {
        public virtual T Create(HttpResponseMessage httpResponse)
        {
            Ensure.That(httpResponse, "httpResponse").IsNotNull();

            return Materialize(
                CreateResponseInstance(),
                httpResponse,
                OnMaterializationOfSuccessfulResponseProperties,
                OnMaterializationOfFailedResponseProperties);
        }

        protected abstract T CreateResponseInstance();

        protected abstract void OnMaterializationOfSuccessfulResponseProperties(T response, HttpResponseMessage httpResponse);

        protected abstract void OnMaterializationOfFailedResponseProperties(T response, HttpResponseMessage httpResponse);
    }
}