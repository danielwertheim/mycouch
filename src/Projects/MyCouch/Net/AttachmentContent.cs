using System;
using System.Net.Http;

namespace MyCouch.Net
{
    [Serializable]
    public class AttachmentContent : StringContent
    {
        public AttachmentContent(string contentType, byte[] content)
            : base(Convert.ToBase64String(content), MyCouchRuntime.DefaultEncoding, contentType) { }
    }
}