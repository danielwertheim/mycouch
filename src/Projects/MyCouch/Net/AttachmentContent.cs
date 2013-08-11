using System;
using System.Net.Http;

namespace MyCouch.Net
{
#if !WinRT
    [Serializable]
#endif
    public class AttachmentContent : StringContent
    {
        public AttachmentContent(string contentType, byte[] content)
            : base(Convert.ToBase64String(content), MyCouchRuntime.DefaultEncoding, contentType) { }
    }
}