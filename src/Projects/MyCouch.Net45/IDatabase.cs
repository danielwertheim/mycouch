using System.Threading.Tasks;
using MyCouch.Requests;
using MyCouch.Responses;

namespace MyCouch
{
    /// <summary>
    /// Used to manage a database.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Gets information about the database.
        /// </summary>
        /// <returns></returns>
        Task<DatabaseResponse> GetAsync();

        /// <summary>
        /// Gets information about the database.
        /// </summary>
        /// <returns></returns>
        Task<DatabaseResponse> GetAsync(GetDatabaseRequest request);

        /// <summary>
        /// Returns the HTTP Headers containing a minimal amount of information
        /// about the specified database. Since the response body is empty,
        /// using the HEAD method is a lightweight way to check if the database
        /// exists already or not.
        /// </summary>
        /// <returns></returns>
        Task<DatabaseResponse> HeadAsync();

        /// <summary>
        /// Returns the HTTP Headers containing a minimal amount of information
        /// about the specified database. Since the response body is empty,
        /// using the HEAD method is a lightweight way to check if the database
        /// exists already or not.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DatabaseResponse> HeadAsync(HeadDatabaseRequest request);
        /// Creates the database, but only if it does not already exist.
        /// </summary>
        Task<DatabaseResponse> PutAsync();

        /// <summary>
        /// Creates the database, but only if it does not already exist.
        /// </summary>
        Task<DatabaseResponse> PutAsync(PutDatabaseRequest request);

        /// <summary>
        /// Deletes the database.
        /// </summary>
        Task<DatabaseResponse> DeleteAsync();

        /// <summary>
        /// Deletes the database.
        /// </summary>
        Task<DatabaseResponse> DeleteAsync(DeleteDatabaseRequest request);

        /// <summary>
        /// Compact, POST /{db}/_compact, requests compaction of the database.
        /// </summary>
        /// <returns></returns>
        Task<DatabaseResponse> CompactAsync();

        /// <summary>
        /// Compact, POST /{db}/_compact, requests compaction of the database.
        /// </summary>
        /// <returns></returns>
        Task<DatabaseResponse> CompactAsync(CompactDatabaseRequest request);

        /// <summary>
        /// Removes view index files that are no longer required by CouchDB as a result of changed views within design documents.
        /// POST /{db}/_view_cleanup
        /// </summary>
        /// <returns></returns>
        Task<DatabaseResponse> ViewCleanup();

        /// <summary>
        /// Removes view index files that are no longer required by CouchDB as a result of changed views within design documents.
        /// POST /{db}/_view_cleanup
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DatabaseResponse> ViewCleanup(ViewCleanupRequest request);
    }
}