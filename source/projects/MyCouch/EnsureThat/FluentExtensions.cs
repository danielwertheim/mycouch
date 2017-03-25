using System.Diagnostics;

namespace MyCouch.EnsureThat
{
    [DebuggerStepThrough]
    public static class FluentExtensions
    {
        public static Param<T> And<T>(this Param<T> param)
        {
            return param;
        } 
    }
}