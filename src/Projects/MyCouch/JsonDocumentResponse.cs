using System;

namespace MyCouch
{
    [Serializable]
    public class JsonDocumentResponse : DocumentResponse
    {
        public string Content { get; set; }

        public override bool IsEmpty
        {
            get { return string.IsNullOrWhiteSpace(Content); }
        }

        protected override string GenerateToStringDebugVersion()
        {
            return string.Format("{0}{1}{0}Content: {2}",
                Environment.NewLine, base.GenerateToStringDebugVersion(), Content ?? "<NULL>");
        }
    }
}