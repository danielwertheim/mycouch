using System;

namespace MyCouch.Responses
{
#if !PCL
    [Serializable]
#endif
    public class DocumentHeaderResponse : Response
    {
        public string Id { get; set; }
        public string Rev { get; set; }

        public override string ToStringDebugVersion()
        {
            return string.Format("{1}{0}Id: {2}{0}Rev: {3}",
                Environment.NewLine,
                base.ToStringDebugVersion(),
                Id ?? NullValueForDebugOutput,
                Rev ?? NullValueForDebugOutput);
        }
    }
}