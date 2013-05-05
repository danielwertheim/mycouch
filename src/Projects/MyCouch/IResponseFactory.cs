using System.Net.Http;

namespace MyCouch
{
    public interface IResponseFactory 
    {
        DatabaseResponse CreateDatabaseResponse(HttpResponseMessage response);
        BulkResponse CreateBulkResponse(HttpResponseMessage response);
        JsonDocumentResponse CreateJsonDocumentResponse(HttpResponseMessage response);
        EntityResponse<T> CreateEntityResponse<T>(HttpResponseMessage response) where T : class;
        JsonViewQueryResponse CreateJsonViewQueryResponse(HttpResponseMessage response);
        ViewQueryResponse<T> CreateViewQueryResponse<T>(HttpResponseMessage response) where T : class;
    }
}