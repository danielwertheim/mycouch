using System.Net.Http;

namespace MyCouch
{
    public interface IResponseFactory 
    {
        DatabaseResponse CreateDatabaseResponse(HttpResponseMessage response);
        DocumentResponse CreateDocumentResponse(HttpResponseMessage response);
        EntityResponse<T> CreateEntityResponse<T>(HttpResponseMessage response) where T : class;
        ViewQueryResponse<T> CreateViewQueryResponse<T>(HttpResponseMessage response) where T : class;
    }
}