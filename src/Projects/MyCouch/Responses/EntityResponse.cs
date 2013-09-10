using System;

namespace MyCouch.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class EntityResponse<T> : Response, IContainDocumentHeader where T : class
    {
        public string Id { get; set; }
        public string Rev { get; set; }
        public T Entity { get; set; }

        public bool IsEmpty
        {
            get { return Entity == null; }
        }

        public override string GenerateToStringDebugVersion()
        {
            return string.Format("{0}{1}{0}Model: {2}{0}IsEmpty: {3}", 
                Environment.NewLine, 
                base.GenerateToStringDebugVersion(), 
                typeof(T).Name,
                IsEmpty);
        }
    }
}