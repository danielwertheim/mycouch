using System;

namespace MyCouch
{
    [Serializable]
    public class EntityResponse<T> : DocumentResponse where T : class
    {
        public T Entity { get; set; }

        public override bool IsEmpty
        {
            get { return Entity == null; }
        }

        public override string GenerateToStringDebugVersion()
        {
            return string.Format("{0}{1}{0}Model: {2}", 
                Environment.NewLine, 
                base.GenerateToStringDebugVersion(), 
                typeof(T).Name);
        }
    }
}