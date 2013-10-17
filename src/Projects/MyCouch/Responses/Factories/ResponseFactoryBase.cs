using System;
using System.IO;
using System.Net.Http;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public abstract class ResponseFactoryBase
    {
        protected readonly ISerializer Serializer;

        protected ResponseFactoryBase(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            Serializer = serializer;
        }

        protected virtual T Materialize<T>(
            T response,
            HttpResponseMessage httpResponse,
            Action<T, HttpResponseMessage> onSuccessfulResponseMaterializer,
            Action<T, HttpResponseMessage> onFailedResponseMaterializer) where T : Response
        {
            response.RequestUri = httpResponse.RequestMessage.RequestUri;
            response.StatusCode = httpResponse.StatusCode;
            response.RequestMethod = httpResponse.RequestMessage.Method;
            response.ContentType = httpResponse.Content.Headers.ContentType.ToString();

            if (response.IsSuccess)
                onSuccessfulResponseMaterializer(response, httpResponse);
            else
                onFailedResponseMaterializer(response, httpResponse);

            return response;
        }

        protected virtual void OnFailedResponse(Response response, HttpResponseMessage httpResponse)
        {
            using (var content = httpResponse.Content.ReadAsStream())
                PopulateFailedInfoFromResponseStream(response, content);
        }

        protected virtual bool ContentShouldHaveIdAndRev(HttpRequestMessage request)
        {
            return
                request.Method == HttpMethod.Post ||
                request.Method == HttpMethod.Put ||
                request.Method == HttpMethod.Delete;
        }

        protected virtual void PopulateFailedInfoFromResponseStream(Response response, Stream content)
        {
            Serializer.Populate(response, content);
        }

        protected virtual void PopulateMissingIdFromRequestUri(DocumentHeaderResponse response, HttpResponseMessage httpResponse)
        {
            if (string.IsNullOrWhiteSpace(response.Id))
                response.Id = httpResponse.RequestMessage.GetUriSegmentByRightOffset();
        }

        protected virtual void PopulateMissingRevFromRequestHeaders(DocumentHeaderResponse response, HttpResponseMessage httpResponse)
        {
            if (string.IsNullOrWhiteSpace(response.Rev))
                response.Rev = httpResponse.Headers.GetETag();
        }

        protected virtual void PopulateDocumentHeaderFromResponseStream(DocumentHeaderResponse response, Stream content)
        {
            Serializer.Populate(response, content);
        }
    }
}