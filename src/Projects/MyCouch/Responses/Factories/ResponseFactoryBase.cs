using System;
using System.IO;
using System.Net.Http;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Responses.Meta;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public abstract class ResponseFactoryBase
    {
        protected readonly SerializationConfiguration SerializationConfiguration;
        protected readonly JsonResponseMapper JsonMapper;

        protected ResponseFactoryBase(SerializationConfiguration serializationConfiguration)
        {
            Ensure.That(serializationConfiguration, "serializationConfiguration").IsNotNull();

            SerializationConfiguration = serializationConfiguration;
            JsonMapper = new JsonResponseMapper(SerializationConfiguration);
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

            if (response.IsSuccess)
                onSuccessfulResponseMaterializer(response, httpResponse);
            else
                onFailedResponseMaterializer(response, httpResponse);

            return response;
        }

        protected virtual void OnFailedResponse(Response response, HttpResponseMessage httpResponse)
        {
            using (var content = httpResponse.Content.ReadAsStream())
                AssignFailedInfoFromResponseStream(response, content);
        }

        protected virtual bool ContentShouldHaveIdAndRev(HttpRequestMessage request)
        {
            return
                request.Method == HttpMethod.Post ||
                request.Method == HttpMethod.Put ||
                request.Method == HttpMethod.Delete;
        }

        protected virtual void AssignFailedInfoFromResponseStream(Response response, Stream content)
        {
            var mappings = new JsonResponseMappings
            {
                {ResponseMeta.Scheme.Error, jr => response.Error = jr.Value.ToString()},
                {ResponseMeta.Scheme.Reason, jr => response.Reason = jr.Value.ToString()}
            };
            JsonMapper.Map(content, mappings);
        }

        //TODO: Rem
        protected virtual void AssignMissingIdFromRequestUri(DocumentHeaderResponse response, HttpResponseMessage httpResponse)
        {
            if (string.IsNullOrWhiteSpace(response.Id))
                response.Id = httpResponse.RequestMessage.GetUriSegmentByRightOffset();
        }

        //TODO: Rem
        protected virtual void AssignMissingRevFromRequestHeaders(DocumentHeaderResponse response, HttpResponseMessage httpResponse)
        {
            if (string.IsNullOrWhiteSpace(response.Rev))
                response.Rev = httpResponse.Headers.GetETag();
        }

        protected virtual void AssignDocumentHeaderFromResponseStream(DocumentHeaderResponse response, Stream content)
        {
            var mappings = new JsonResponseMappings
            {
                {ResponseMeta.Scheme.Id, jr => response.Id = jr.Value.ToString()},
                {ResponseMeta.Scheme.Rev, jr => response.Rev = jr.Value.ToString()}
            };
            JsonMapper.Map(content, mappings);
        }
    }
}