using System;

namespace MyCouch.Responses
{
    public class AttachmentResponse : BytesResponse,
        IDocumentHeader
    {
        public string Id { get; set; }
        public string Rev { get; set; }
        public string Name { get; set; }
        
        public override string ToStringDebugVersion()
        {
            return string.Format("{1}{0}Id: {2}{0}Rev: {3}{0}Name: {4}",
                Environment.NewLine, 
                base.ToStringDebugVersion(),
                Id ?? NullValueForDebugOutput,
                Rev ?? NullValueForDebugOutput,
                Name);
        }
    }
}