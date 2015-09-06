using System;

namespace MyCouch.Responses
{
#if !PCL && !vNext
    [Serializable]
#endif
    public abstract class TextResponse : ContentResponse<string>
    {
        public override bool IsEmpty
        {
            get { return string.IsNullOrWhiteSpace(Content); }
        }

        public override string ToStringDebugVersion()
        {
            return string.Format("{1}{0}Content: {2}",
                Environment.NewLine,
                base.ToStringDebugVersion(),
                Content ?? NullValueForDebugOutput);
        }
    }
}