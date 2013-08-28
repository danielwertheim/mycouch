using System;

namespace MyCouch.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class DocumentResponse : DocumentHeaderResponse
    {
        public string Content { get; set; }

        public bool IsEmpty
        {
            get { return string.IsNullOrWhiteSpace(Content); }
        }

        public override string GenerateToStringDebugVersion()
        {
            return string.Format("{0}{1}{0}IsEmpty: {2}{0}Content: {3}",
                Environment.NewLine, 
                base.GenerateToStringDebugVersion(),
                IsEmpty, 
                Content ?? "<NULL>");
        }
    }
}