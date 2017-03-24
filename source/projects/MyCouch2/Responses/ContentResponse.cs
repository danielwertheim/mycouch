using System;

namespace MyCouch.Responses
{
#if net45
    [Serializable]
#endif
    public abstract class ContentResponse<TContent> : Response
    {
        public TContent Content { get; set; }
        public abstract bool IsEmpty { get; }

        public override string ToStringDebugVersion()
        {
            return string.Format("{1}{0}IsEmpty: {2}",
                Environment.NewLine,
                base.ToStringDebugVersion(),
                IsEmpty);
        }
    }
}