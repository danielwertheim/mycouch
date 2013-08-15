#if net45 || NETFX_CORE
using System.Runtime.CompilerServices;
#elif net40
using Microsoft.Runtime.CompilerServices;
#endif
using System.Threading.Tasks;

namespace MyCouch.Extensions
{
    internal static class TaskExtensions
    {
        internal static ConfiguredTaskAwaitable<T> ForAwait<T>(this Task<T> t)
        {
            return t.ConfigureAwait(false);
        }
    }
}