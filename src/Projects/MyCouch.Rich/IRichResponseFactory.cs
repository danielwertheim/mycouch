using System.Net.Http;

namespace MyCouch.Rich
{
    public interface IRichResponseFactory : IResponseFactory 
    {
        EntityResponse<T> CreateEntityResponse<T>(HttpResponseMessage response) where T : class;
    }
}