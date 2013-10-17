using System.IO;

namespace MyCouch.Serialization
{
    public interface ISerializer
    {
        string Serialize<T>(T item) where T : class;
        T Deserialize<T>(string data) where T : class;
        T Deserialize<T>(Stream data) where T : class;
        void Populate<T>(T item, Stream data) where T : class;
    }
}