using System.Threading;
using System.Threading.Tasks;
using MyCouch.Requests;
using MyCouch.Responses;

namespace MyCouch
{
    public interface IDatabases
    {
        /// <summary>
        /// Gets information about the database.
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<GetDatabaseResponse> GetAsync(string dbName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets information about the database.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<GetDatabaseResponse> GetAsync(GetDatabaseRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the HTTP Headers containing a minimal amount of information
        /// about the specified database. Since the response body is empty,
        /// using the HEAD method is a lightweight way to check if the database
        /// exists already or not.
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> HeadAsync(string dbName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the HTTP Headers containing a minimal amount of information
        /// about the specified database. Since the response body is empty,
        /// using the HEAD method is a lightweight way to check if the database
        /// exists already or not.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> HeadAsync(HeadDatabaseRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the database, but only if it does not already exist.
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> PutAsync(string dbName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the database, but only if it does not already exist.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> PutAsync(PutDatabaseRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the database.
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> DeleteAsync(string dbName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the database.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> DeleteAsync(DeleteDatabaseRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Requests compaction of the database.
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> CompactAsync(string dbName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Requests compaction of the database.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> CompactAsync(CompactDatabaseRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes view index files that are no longer required by CouchDB as a
        /// result of changed views within design documents.
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> ViewCleanupAsync(string dbName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes view index files that are no longer required by CouchDB as a
        /// result of changed views within design documents.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> ViewCleanupAsync(ViewCleanupRequest request, CancellationToken cancellationToken = default);
    }
}