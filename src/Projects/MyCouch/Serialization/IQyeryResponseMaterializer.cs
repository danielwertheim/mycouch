using System.IO;
using MyCouch.Responses;

namespace MyCouch.Serialization
{
    public interface IQyeryResponseMaterializer
    {
        void Populate<T>(ViewQueryResponse<T> response, Stream data) where T : class;
    }
}