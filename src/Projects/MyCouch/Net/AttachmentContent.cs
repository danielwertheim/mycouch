using System;
using System.Net.Http;

namespace MyCouch.Net
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class AttachmentContent : StringContent
    {
        public AttachmentContent(string contentType, byte[] content)
            : base(Convert.ToBase64String(content), MyCouchRuntime.DefaultEncoding, contentType) { }
    }
}