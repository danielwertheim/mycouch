using System;
using System.Diagnostics;
using MyCouch.EnsureThat.Resources;

namespace MyCouch.EnsureThat
{
    public static class EnsureGuidExtensions
    {
        [DebuggerStepThrough]
        public static Param<Guid> IsNotEmpty(this Param<Guid> param)
        {
            if (Guid.Empty.Equals(param.Value))
                throw ExceptionFactory.CreateForParamValidation(param, ExceptionMessages.EnsureExtensions_IsEmptyGuid);

            return param;
        }
    }
}