using System.Runtime.CompilerServices;
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