using MyCouch.Serialization;

namespace MyCouch.Rich.Serialization
{
    public interface IRichSerializer : ISerializer
    {
        string SerializeEntity<T>(T entity) where T : class;
    }
}