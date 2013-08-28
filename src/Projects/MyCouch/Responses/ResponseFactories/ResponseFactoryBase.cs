using System;
using System.Net.Http;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.ResponseFactories
{
    public abstract class ResponseFactoryBase
    {
        protected readonly IResponseMaterializer ResponseMaterializer;

        protected ResponseFactoryBase(IResponseMaterializer responseMaterializer)
        {
            Ensure.That(responseMaterializer, "responseMaterializer").IsNotNull();

            ResponseMaterializer = responseMaterializer;
        }

        protected virtual T CreateResponse<T>(
            HttpResponseMessage response,
            Action<HttpResponseMessage, T> onSuccessfulResponseContentMaterializer,
            Action<HttpResponseMessage, T> onFailedResponseContentMaterializer) where T : Response, new()
        {
            var result = new T
            {
                RequestUri = response.RequestMessage.RequestUri,
                StatusCode = response.StatusCode,
                RequestMethod = response.RequestMessage.Method
            };

            if (result.IsSuccess)
                onSuccessfulResponseContentMaterializer(response, result);
            else
                onFailedResponseContentMaterializer(response, result);

            return result;
        }

        protected virtual void OnFailedResponseContentMaterializer<T>(HttpResponseMessage response, T result) where T : IResponse
        {
            using (var content = response.Content.ReadAsStream())
                ResponseMaterializer.PopulateFailedResponse(result, content);
        }

        protected virtual bool ContentShouldHaveIdAndRev(HttpRequestMessage request)
        {
            return
                request.Method == HttpMethod.Post ||
                request.Method == HttpMethod.Put ||
                request.Method == HttpMethod.Delete;
        }

        protected virtual void AssignMissingIdFromRequestUri(HttpResponseMessage response, DocumentHeaderResponse result)
        {
            if (string.IsNullOrWhiteSpace(result.Id))
                result.Id = response.RequestMessage.GetUriSegmentByRightOffset();
        }

        protected virtual void AssignMissingIdFromRequestUri(HttpResponseMessage response, AttachmentResponse result)
        {
            if (string.IsNullOrWhiteSpace(result.Id))
                result.Id = response.RequestMessage.GetUriSegmentByRightOffset(1);
        }

        protected virtual void AssignMissingNameFromRequestUri(HttpResponseMessage response, AttachmentResponse result)
        {
            if (string.IsNullOrWhiteSpace(result.Name))
                result.Name = response.RequestMessage.GetUriSegmentByRightOffset();
        }

        protected virtual void AssignMissingRevFromRequestHeaders(HttpResponseMessage response, DocumentHeaderResponse result)
        {
            if (string.IsNullOrWhiteSpace(result.Rev))
                result.Rev = response.Headers.GetETag();
        }
    }
}