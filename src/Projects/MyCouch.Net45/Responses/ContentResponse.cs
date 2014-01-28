using System;

namespace MyCouch.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public abstract class ContentResponse : Response
    {
        public string Content { get; set; }

        public override string ToStringDebugVersion()
        {
            return string.Format("{1}{0}Content:{2}",
                Environment.NewLine,
                base.ToStringDebugVersion(),
                Content ?? "<NULL>");
        }
    }
}