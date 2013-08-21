using MyCouch.Rich.Serialization;
using MyCouch.Schemes;

namespace MyCouch.Rich
{
    public interface IRichClient : IClient
    {
        /// <summary>
        /// The Serializer associated with this client instance. Use this is you want
        /// to serialize or deserialize using the same behavior that the provider
        /// has.
        /// </summary>
        IRichSerializer Serializer { get; set; }

        /// <summary>
        /// Factory used to build <see cref="IResponse"/>.
        /// </summary>
        IRichResponseFactory ResponseFactory { get; set; }

        /// <summary>
        /// Used to get and set specific members of entities when you are using the
        /// typed API.
        /// </summary>
        IEntityReflector EntityReflector { get; set; }

        /// <summary>
        /// Entity oriented API operations, for accessing and managing documents as entities.
        /// </summary>
        IEntities Entities { get; }
    }
}