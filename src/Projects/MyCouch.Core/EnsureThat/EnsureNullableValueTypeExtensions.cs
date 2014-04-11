using System.Diagnostics;
using MyCouch.EnsureThat.Resources;

namespace MyCouch.EnsureThat
{
    public static class EnsureNullableValueTypeExtensions
    {
        [DebuggerStepThrough]
        public static Param<T?> IsNotNull<T>(this Param<T?> param) where T : struct
        {
            if (param.Value == null || !param.Value.HasValue)
                throw ExceptionFactory.CreateForParamNullValidation(param, ExceptionMessages.EnsureExtensions_IsNotNull);

            return param;
        }
    }
}