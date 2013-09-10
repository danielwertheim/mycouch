using System;
using System.IO;
using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class AttachmentResponseFactory : ResponseFactoryBase
    {
        public AttachmentResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration) { }

        public virtual AttachmentResponse Create(HttpResponseMessage response)
        {
            return BuildResponse(new AttachmentResponse(), response, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse(HttpResponseMessage response, AttachmentResponse result)
        {
            using (var content = response.Content.ReadAsStream())
            {
                AssignMissingIdFromRequestUri(response, result);
                AssignMissingNameFromRequestUri(response, result);
                AssignMissingRevFromRequestHeaders(response, result);

                content.Position = 0;
                using (var reader = new StreamReader(content, MyCouchRuntime.DefaultEncoding))
                {
                    result.Content = Convert.FromBase64String(reader.ReadToEnd());
                }
            }
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
    }
}