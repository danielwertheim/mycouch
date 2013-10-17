using MyCouch.EntitySchemes;
using MyCouch.Serialization;

namespace MyCouch.Requests.Factories
{
    public abstract class EntityHttpRequestFactoryBase : HttpRequestFactoryBase
    {
        protected readonly IEntitySerializer Serializer;
        protected readonly IEntityReflector Reflector;

        protected EntityHttpRequestFactoryBase(IConnection connection, IEntitySerializer serializer, IEntityReflector reflector)
            : base(connection)
        {
            Serializer = serializer;
            Reflector = reflector;
        }

        protected virtual string GenerateRequestUrl(string id = null, string rev = null)
        {
            return string.Format("{0}/{1}{2}",
                Connection.Address,
                id ?? string.Empty,
                rev == null ? string.Empty : string.Concat("?rev=", rev));
        }

        protected virtual string SerializeEntity<T>(T entity) where T : class
        {
            return Serializer.Serialize(entity);
        }
    }
}