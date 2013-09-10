using System;

namespace MyCouch.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class DocumentResponse : Response, IContainDocumentHeader
    {
        public string Id { get; set; }
        public string Rev { get; set; }
        public string Content { get; set; }

        public bool IsEmpty
        {
            get { return string.IsNullOrWhiteSpace(Content); }
        }

        public override string ToStringDebugVersion()
        {
            return string.Format("{0}{1}{0}IsEmpty: {2}{0}Content: {3}",
                Environment.NewLine, 
                base.ToStringDebugVersion(),
                IsEmpty, 
                Content ?? "<NULL>");
        }
    }
}