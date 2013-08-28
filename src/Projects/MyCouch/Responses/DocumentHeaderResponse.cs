using System;

namespace MyCouch.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class DocumentHeaderResponse : Response
    {
        public string Id { get; set; }
        public string Rev { get; set; }

        public override string GenerateToStringDebugVersion()
        {
            return string.Format("{0}{1}{0}Id: {2}{0}Rev: {3}",
                Environment.NewLine,
                base.GenerateToStringDebugVersion(),
                Id ?? "<NULL>",
                Rev ?? "<NULL>");
        }
    }
}