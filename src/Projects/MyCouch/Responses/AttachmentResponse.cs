using System;

namespace MyCouch.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class AttachmentResponse : Response, IContainDocumentHeader
    {
        public string Id { get; set; }
        public string Rev { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }

        public bool IsEmpty
        {
            get { return Content == null || Content.Length < 1; }
        }

        public override string GenerateToStringDebugVersion()
        {
            return string.Format("{0}{1}{0}IsEmpty: {2}{0}Content: {3}",
                Environment.NewLine, 
                base.GenerateToStringDebugVersion(),
                IsEmpty,
                IsEmpty ? "<NULL>" : Convert.ToBase64String(Content));
        }
    }
}