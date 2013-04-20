using System.Net.Http;

namespace MyCouch
{
    public interface IDbClientResponseFactory 
    {
        DatabaseResponse CreateDatabaseResponse(HttpResponseMessage response);
        DocumentResponse CreateDocumentResponse(HttpResponseMessage response);
        EntityResponse<T> CreateEntityResponse<T>(HttpResponseMessage response) where T : class;
        ViewQueryResponse CreateViewQueryResponse(HttpResponseMessage response);
        ViewQueryResponse<T> CreateViewQueryResponse<T>(HttpResponseMessage response) where T : class;
    }
}