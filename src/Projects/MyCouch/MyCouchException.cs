using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MyCouch
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class MyCouchException : AggregateException
    {
        public MyCouchException(string message)
            : base(message)
        { }

        public MyCouchException(string message, IEnumerable<Exception> innerExceptions)
            : base(message, innerExceptions)
        { }

#if !NETFX_CORE
        protected MyCouchException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
#endif
    }
}