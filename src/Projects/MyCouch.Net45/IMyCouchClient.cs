using System;
using MyCouch.Serialization;

namespace MyCouch
{
    /// <summary>
    /// Connects to a DB instance rather than a server instance
    /// as <see cref="IMyCouchServerClient"/> does.
    /// Alternative API is <see cref="IMyCouchStore"/>.
    /// </summary>
    public interface IMyCouchClient : IDisposable
    {
        /// <summary>
        /// Represents the DbName that MyCouch has parsed out
        /// from the URL.
        /// </summary>
        string DbName { get; }

        /// <summary>
        /// The underlying <see cref="IConnection"/> used to communicate with CouchDb.
        /// </summary>
        IConnection Connection { get; }
        
        /// <summary>
        /// The Serializer associated with this client instance. Use this if you want
        /// to serialize or deserialize using the same behavior that the provider has.
        /// </summary>
        /// <remarks>If you want a serializer that supports entity conventions, check <see cref="IEntities.Serializer"/></remarks>
        ISerializer Serializer { get; }

        /// <summary>
        /// Changes oriented API operations, for getting or subscribinh to changes in the database.
        /// </summary>
        IChanges Changes { get; }

        /// <summary>
        /// Attachment oriented API operations, for accessing and managing attachments to documents.
        /// </summary>
        IAttachments Attachments { get; }

        /// <summary>
        /// Database oriented API operations.
        /// </summary>
        IDatabase Database { get; }

        /// <summary>
        /// Document oriented API operations, for accessing and managing RAW documents.
        /// </summary>
        IDocuments Documents { get; }

        /// <summary>
        /// Entity oriented API operations, for accessing and managing documents as entities.
        /// </summary>
        IEntities Entities { get; }

        /// <summary>
        /// View oriented API operations, for accessing and managing views.
        /// </summary>
        IViews Views { get; }
    }
}