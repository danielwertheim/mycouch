using System;

namespace MyCouch.Responses
{
#if !PCL
    [Serializable]
#endif
    public class ListQueryResponse : TextResponse
    {
        public string ETag { get; set; }

        public override string ToStringDebugVersion()
        {
            return string.Format("{1}{0}ETag: {2}",
                Environment.NewLine,
                base.ToStringDebugVersion(),
                ETag ?? NullValueForDebugOutput);
        }
    }
}