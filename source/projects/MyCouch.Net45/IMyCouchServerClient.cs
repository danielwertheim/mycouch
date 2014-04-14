using System;
using MyCouch.Serialization;

namespace MyCouch
{
    /// <summary>
    /// Connects to a server instance rather than a DB instance
    /// as <see cref="IMyCouchClient"/> or <see cref="IMyCouchStore"/>
    /// does.
    /// </summary>
    public interface IMyCouchServerClient : IDisposable
    {
        /// <summary>
        /// The underlying <see cref="IServerClientConnection"/> used to communicate with CouchDb.
        /// </summary>
        IServerClientConnection Connection { get; }

        /// <summary>
        /// The Serializer associated with this client instance. Use this if you want
        /// to serialize or deserialize using the same behavior that the provider has.
        /// </summary>
        /// <remarks></remarks>
        ISerializer Serializer { get; }

        /// <summary>
        /// Database oriented API operations.
        /// </summary>
        IDatabases Databases { get; }
    }
}