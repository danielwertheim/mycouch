using System.IO;

namespace MyCouch.Serialization
{
    public interface ISerializer
    {
        string Serialize<T>(T item) where T : class;
        string SerializeEntity<T>(T entity) where T : class;
        
        T Deserialize<T>(string data) where T : class;
        T DeserializeEntity<T>(Stream data) where T : class;

        void PopulateSingleDocumentResponse<T>(T response, Stream data) where T : SingleDocumentResponse;
        void PopulateViewQueryResponse<T>(ViewQueryResponse<T> item, Stream data) where T : class;
        void PopulateFailedResponse<T>(T response, Stream data) where T : Response;
    }
}