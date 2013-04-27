using System;
using MyCouch.Net;
using MyCouch.Schemes;
using MyCouch.Serialization;

namespace MyCouch
{
    public interface IClient : IDisposable
    {
        /// <summary>
        /// The underlying <see cref="IConnection"/> used to communicate with CouchDb.
        /// </summary>
        IConnection Connection { get; }
        
        /// <summary>
        /// The Serializer associated with this client instance. Use this is you want
        /// to serialize or deserialize using the same behavior that the provider
        /// has.
        /// </summary>
        ISerializer Serializer { get; set; }

        /// <summary>
        /// Factory used to build <see cref="IResponse"/>.
        /// </summary>
        IResponseFactory ResponseFactory { get; set; }

        /// <summary>
        /// Used to get and set specific members of entities when you are using the
        /// typed API.
        /// </summary>
        IEntityReflector EntityReflector { get; set; }

        /// <summary>
        /// Database oriented APU operations, for managing databases.
        /// </summary>
        IDatabases Databases { get; }

        /// <summary>
        /// Document oriented API operations, for accessing and managing documents.
        /// </summary>
        IDocuments Documents { get; }
        
        /// <summary>
        /// View oriented API operations, for accessing and managing views.
        /// </summary>
        IViews Views { get; }
    }
}