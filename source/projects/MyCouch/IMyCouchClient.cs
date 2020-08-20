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
        /// The underlying <see cref="IDbConnection"/> used to communicate with CouchDb.
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// More or less Vanilla configured serializer.
        /// </summary>
        /// <remarks>If you want a serializer that supports entity conventions etc and is used
        /// to work with results from <see cref="Entities"/> and
        /// <see cref="Views"/>, then check <see cref="DocumentSerializer"/></remarks>
        ISerializer Serializer { get; }

        /// <summary>
        /// Supports entity conventions etc that is used in the contexts of e.g.
        /// <see cref="Views"/>
        /// <see cref="Entities"/>
        /// <see cref="Documents"/>
        /// </summary>
        /// <remarks>For vanilla serializer, <see cref="Serializer"/>.</remarks>
        ISerializer DocumentSerializer { get; }

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
        /// Used to access the Query API, inspired by MongoDB syntax.
        /// </summary>
        IQueries Queries { get; }

        ///// <summary>
        ///// Used to access Search Indexes.
        ///// </summary>
        ISearches Searches { get; }

        /// <summary>
        /// View oriented API operations, for accessing and managing views.
        /// </summary>
        IViews Views { get; }
    }
}