using System.IO;
using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Responses.Meta;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public abstract class DocumentHeaderResponseFactoryBase : ResponseFactoryBase
    {
        protected DocumentHeaderResponseFactoryBase(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration) { }

        protected virtual void OnSuccessfulResponse(HttpResponseMessage response, DocumentHeaderResponse result)
        {
            if (response.RequestMessage.Method == HttpMethod.Head)
            {
                AssignMissingIdFromRequestUri(response, result);
                AssignMissingRevFromRequestHeaders(response, result);

                return;
            }

            using (var content = response.Content.ReadAsStream())
                PopulateDocumentHeaderResponse(result, content);
        }

        protected virtual void PopulateDocumentHeaderResponse(DocumentHeaderResponse result, Stream content)
        {
            var mappings = new JsonResponseMappings
            {
                {ResponseMeta.Scheme.Id, jr => result.Error = jr.Value.ToString()},
                {ResponseMeta.Scheme.Rev, jr => result.Reason = jr.Value.ToString()}
            };
            JsonMapper.Map(content, mappings);
        }

        protected virtual void OnFailedResponse(HttpResponseMessage response, DocumentHeaderResponse result)
        {
            base.OnFailedResponse(response, result);

            AssignMissingIdFromRequestUri(response, result);
        }
    }
}