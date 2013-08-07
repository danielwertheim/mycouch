using System.Runtime.CompilerServices;
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