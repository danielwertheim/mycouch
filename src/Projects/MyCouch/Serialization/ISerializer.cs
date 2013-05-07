using System.IO;

namespace MyCouch.Serialization
{
    public interface ISerializer
    {
        string Serialize<T>(T item) where T : class;
        string SerializeEntity<T>(T entity) where T : class;
        
        T Deserialize<T>(string data) where T : class;
        T Deserialize<T>(Stream data) where T : class;

        void PopulateFailedResponse<T>(T response, Stream data) where T : Response;
        void PopulateBulkResponse(BulkResponse response, Stream data);
        void PopulateCopyDocumentResponse(CopyDocumentResponse response, Stream data);
        void PopulateReplaceDocumentResponse(ReplaceDocumentResponse response, Stream data);
        void PopulateDocumentResponse<T>(T response, Stream data) where T : DocumentResponse;
        void PopulateViewQueryResponse<T>(ViewQueryResponse<T> item, Stream data) where T : class;
    }
}