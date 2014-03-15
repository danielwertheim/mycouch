using System;

namespace MyCouch.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class EntityResponse<T> : ContentResponse<T> where T : class
    {
        public string Id { get; set; }
        public string Rev { get; set; }

        [Obsolete("Use Content instead")]
        public T Entity
        {
            get { return Content; } 
            set { Content = value; }
        }

        public override bool IsEmpty
        {
            get { return Content == null; }
        }

        public override string ToStringDebugVersion()
        {
            return string.Format("{1}{0}Content: {2}{0}Model: {3}{0}Id: {4}{0}Rev: {5}",
                Environment.NewLine,
                base.ToStringDebugVersion(),
                Content == null ? NullValueForDebugOutput : "<ENTITY>",
                typeof(T).Name,
                Id ?? NullValueForDebugOutput,
                Rev ?? NullValueForDebugOutput);
        }
    }
}