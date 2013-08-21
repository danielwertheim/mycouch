using System;
using MyCouch.Net;
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
        ISerializer Serializer { get; }

        /// <summary>
        /// Database oriented API operations.
        /// </summary>
        IDatabase Database { get; }

        /// <summary>
        /// Document oriented API operations, for accessing and managing RAW documents.
        /// </summary>
        IDocuments Documents { get; }

        /// <summary>
        /// Attachment oriented API operations, for accessing and managing attachments to documents.
        /// </summary>
        IAttachments Attachments { get; }
        
        /// <summary>
        /// View oriented API operations, for accessing and managing views.
        /// </summary>
        IViews Views { get; }
    }
}