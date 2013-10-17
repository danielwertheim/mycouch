using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class AttachmentResponseFactory : ResponseFactoryBase
    {
        public AttachmentResponseFactory(ISerializer serializer)
            : base(serializer)
        {
        }

        public virtual AttachmentResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new AttachmentResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse(AttachmentResponse response, HttpResponseMessage httpResponse)
        {
            using (var content = httpResponse.Content.ReadAsStream())
            {
                PopulateMissingIdFromRequestUri(response, httpResponse);
                PopulateMissingNameFromRequestUri(response, httpResponse);
                PopulateMissingRevFromRequestHeaders(response, httpResponse);

                content.Position = 0;

                response.Content = httpResponse.Content.ReadAsByteArrayAsync().Result;
            }
        }

        protected override void PopulateMissingIdFromRequestUri(DocumentHeaderResponse response, HttpResponseMessage httpResponse)
        {
            if (string.IsNullOrWhiteSpace(response.Id))
                response.Id = httpResponse.RequestMessage.GetUriSegmentByRightOffset(1);
        }

        protected virtual void PopulateMissingNameFromRequestUri(AttachmentResponse response, HttpResponseMessage httpResponse)
        {
            if (string.IsNullOrWhiteSpace(response.Name))
                response.Name = httpResponse.RequestMessage.GetUriSegmentByRightOffset();
        }
    }
}