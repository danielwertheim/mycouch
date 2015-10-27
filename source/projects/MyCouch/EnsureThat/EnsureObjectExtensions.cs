using System.Diagnostics;

namespace MyCouch.EnsureThat
{
    public static class EnsureObjectExtensions
    {
        [DebuggerStepThrough]
        public static Param<T> IsNotNull<T>(this Param<T> param, Throws<T>.ExceptionFnConfig exceptionFn = null) where T : class
        {
            if (param.Value == null)
            {
                if (exceptionFn != null)
                    throw exceptionFn(Throws<T>.Instance)(param);

                throw ExceptionFactory.CreateForParamNullValidation(param, ExceptionMessages.EnsureExtensions_IsNotNull);
            }

            return param;
        }
    }
}