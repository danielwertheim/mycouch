using MyCouch.EnsureThat;
using MyCouch.EntitySchemes;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch.Requests.Factories
{
    public abstract class EntityHttpRequestFactoryBase : HttpRequestFactoryBase
    {
        protected ConstantRequestUrlGenerator RequestUrlGenerator { get; private set; }
        protected IEntitySerializer Serializer { get; private set; }
        protected IEntityReflector Reflector { get; private set; }

        protected EntityHttpRequestFactoryBase(IDbClientConnection connection, IEntitySerializer serializer, IEntityReflector reflector)
        {
            Ensure.That(connection, "connection").IsNotNull();
            Ensure.That(serializer, "serializer").IsNotNull();
            Ensure.That(reflector, "reflector").IsNotNull();

            RequestUrlGenerator = new ConstantRequestUrlGenerator(connection.Address, connection.DbName);
            Serializer = serializer;
            Reflector = reflector;
        }

        protected IConnection Connection { get; private set; }

        protected virtual string GenerateRequestUrl(string id = null, string rev = null)
        {
            return string.Format("{0}/{1}{2}",
                RequestUrlGenerator.Generate(),
                id ?? string.Empty,
                rev == null ? string.Empty : string.Concat("?rev=", rev));
        }

        protected virtual string SerializeEntity<T>(T entity) where T : class
        {
            return Serializer.Serialize(entity);
        }
    }
}