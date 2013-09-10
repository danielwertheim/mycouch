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

        protected virtual T BuildResponse<T>(
            T result,
            HttpResponseMessage response,
            Action<HttpResponseMessage, T> onSuccessfulResponseContentMaterializer,
            Action<HttpResponseMessage, T> onFailedResponseContentMaterializer) where T : Response
        {
            result.RequestUri = response.RequestMessage.RequestUri;
            result.StatusCode = response.StatusCode;
            result.RequestMethod = response.RequestMessage.Method;

            if (result.IsSuccess)
                onSuccessfulResponseContentMaterializer(response, result);
            else
                onFailedResponseContentMaterializer(response, result);

            return result;
        }

        protected virtual void OnFailedResponse(HttpResponseMessage response, Response result)
        {
            using (var content = response.Content.ReadAsStream())
                PopulateFailedResponse(result, content);
        }

        protected virtual void PopulateFailedResponse(Response result, Stream content)
        {
            var mappings = new JsonResponseMappings
            {
                {ResponseMeta.Scheme.Error, jr => result.Error = jr.Value.ToString()},
                {ResponseMeta.Scheme.Reason, jr => result.Reason = jr.Value.ToString()}
            };
            JsonMapper.Map(content, mappings);
        }

        protected virtual void PopulateDocumentHeaderResponse(IContainDocumentHeader result, Stream content)
        {
            var mappings = new JsonResponseMappings
            {
                {ResponseMeta.Scheme.Id, jr => result.Id = jr.Value.ToString()},
                {ResponseMeta.Scheme.Rev, jr => result.Rev = jr.Value.ToString()}
            };
            JsonMapper.Map(content, mappings);
        }

        protected virtual bool ContentShouldHaveIdAndRev(HttpRequestMessage request)
        {
            return
                request.Method == HttpMethod.Post ||
                request.Method == HttpMethod.Put ||
                request.Method == HttpMethod.Delete;
        }

        protected virtual void AssignMissingIdFromRequestUri(HttpResponseMessage response, IContainDocumentHeader result)
        {
            if (string.IsNullOrWhiteSpace(result.Id))
                result.Id = response.RequestMessage.GetUriSegmentByRightOffset();
        }

        protected virtual void AssignMissingRevFromRequestHeaders(HttpResponseMessage response, IContainDocumentHeader result)
        {
            if (string.IsNullOrWhiteSpace(result.Rev))
                result.Rev = response.Headers.GetETag();
        }
    }
}