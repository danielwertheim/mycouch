using System;
using System.IO;
using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class AttachmentResponseFactory : ResponseFactoryBase
    {
        public AttachmentResponseFactory(IResponseMaterializer responseMaterializer) : base(responseMaterializer) { }

        public virtual AttachmentResponse Create(HttpResponseMessage response)
        {
            return CreateResponse<AttachmentResponse>(response, OnSuccessfulAttachmentResponseContentMaterializer, OnFailedResponseContentMaterializer);
        }

        protected virtual void OnSuccessfulAttachmentResponseContentMaterializer(HttpResponseMessage response, AttachmentResponse result)
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
    }
}