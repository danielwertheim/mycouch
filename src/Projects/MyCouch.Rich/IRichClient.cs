using MyCouch.Rich.Serialization;

namespace MyCouch.Rich
{
    public interface IRichClient : IClient
    {
        /// <summary>
        /// The Serializer associated with this client instance. Use this is you want
        /// to serialize or deserialize using the same behavior that the provider
        /// has.
        /// </summary>
        new IRichSerializer Serializer { get; }

        /// <summary>
        /// Entity oriented API operations, for accessing and managing documents as entities.
        /// </summary>
        IEntities Entities { get; }
    }
}