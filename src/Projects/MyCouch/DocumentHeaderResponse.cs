using System;

namespace MyCouch
{
    [Serializable]
    public class DocumentHeaderResponse : Response, IDocumentHeaderResponse
    {
        public string Id { get; set; }
        public string Rev { get; set; }

        protected override string GenerateToStringDebugVersion()
        {
            return string.Format("{0}{1}{0}Id: {2}{0}Rev: {3}",
                Environment.NewLine,
                base.GenerateToStringDebugVersion(),
                Id ?? "<NULL>",
                Rev ?? "<NULL>");
        }
    }
}