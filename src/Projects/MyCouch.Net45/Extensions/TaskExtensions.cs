#if net45 || NETFX_CORE
using System.Runtime.CompilerServices;
#elif net40
using Microsoft.Runtime.CompilerServices;
#endif
using System.Threading.Tasks;

namespace MyCouch.Extensions
{
    public static class TaskExtensions
    {
        public static ConfiguredTaskAwaitable<T> ForAwait<T>(this Task<T> t)
        {
            return t.ConfigureAwait(false);
        }
    }
}