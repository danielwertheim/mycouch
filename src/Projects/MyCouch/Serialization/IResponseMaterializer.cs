using System.IO;
using MyCouch.Responses;

namespace MyCouch.Serialization
{
    public interface IResponseMaterializer
    {
        void PopulateViewQueryResponse<T>(ViewQueryResponse<T> response, Stream data) where T : class;
    }
}