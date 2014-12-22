using MyCouch.Requests;
using MyCouch.Responses;
using System.Threading.Tasks;

namespace MyCouch
{
    public interface IShows
    {
        Task<RawResponse> QueryRawAsync(ShowRequest request);
    }
}
