using System.Threading.Tasks;
using MyCouch.Requests;
using MyCouch.Responses;

namespace MyCouch
{
    public interface ILists
    {
        //TODO: Document
        Task<ListQueryResponse> QueryAsync(QueryListRequest listQuery);
    }
}