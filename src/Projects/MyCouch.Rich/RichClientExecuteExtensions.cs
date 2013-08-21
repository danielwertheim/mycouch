using System.Threading.Tasks;
using MyCouch.Commands;

namespace MyCouch.Rich
{
    public static class RichClientExecuteExtensions
    {
        public static Task<EntityResponse<T>> ExecuteAsync<T>(this IRichClient client, GetEntityCommand cmd) where T : class
        {
            return client.Entities.GetAsync<T>(cmd);
        }
    }
}