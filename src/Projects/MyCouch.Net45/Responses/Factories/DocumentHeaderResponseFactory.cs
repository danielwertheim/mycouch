using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class DocumentHeaderResponseFactory : ResponseFactoryBase
    {
        public DocumentHeaderResponseFactory(ISerializer serializer)
            : base(serializer)
        { }

        public virtual DocumentHeaderResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new DocumentHeaderResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected async virtual void OnSuccessfulResponse(DocumentHeaderResponse response, HttpResponseMessage httpResponse)
        {
            if (httpResponse.RequestMessage.Method == HttpMethod.Head)
            {
                PopulateMissingIdFromRequestUri(response, httpResponse);
                PopulateMissingRevFromRequestHeaders(response, httpResponse);

                return;
            }

            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
                PopulateDocumentHeaderFromResponseStream(response, content);
        }

        protected virtual void OnFailedResponse(DocumentHeaderResponse response, HttpResponseMessage httpResponse)
        {
            base.OnFailedResponse(response, httpResponse);

            PopulateMissingIdFromRequestUri(response, httpResponse);
        }
    }
}