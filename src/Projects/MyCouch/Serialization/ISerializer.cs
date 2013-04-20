using System.Collections.Generic;
using System.IO;

namespace MyCouch.Serialization
{
    public interface ISerializer
    {
        string Serialize<T>(T item) where T : class;
        string SerializeEntity<T>(T entity) where T : class;
        T Deserialize<T>(Stream data) where T : class;
        T Deserialize<T>(string data) where T : class;
        IEnumerable<T> Deserialize<T>(IEnumerable<string> data) where T : class;
    }
}