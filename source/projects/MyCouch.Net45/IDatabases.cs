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
        /// <returns></returns>
        Task<GetDatabaseResponse> GetAsync(string dbName);

        /// <summary>
        /// Gets information about the database.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<GetDatabaseResponse> GetAsync(GetDatabaseRequest request);

        /// <summary>
        /// Returns the HTTP Headers containing a minimal amount of information
        /// about the specified database. Since the response body is empty,
        /// using the HEAD method is a lightweight way to check if the database
        /// exists already or not.
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> HeadAsync(string dbName);

        /// <summary>
        /// Returns the HTTP Headers containing a minimal amount of information
        /// about the specified database. Since the response body is empty,
        /// using the HEAD method is a lightweight way to check if the database
        /// exists already or not.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> HeadAsync(HeadDatabaseRequest request);

        /// <summary>
        /// Creates the database, but only if it does not already exist.
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> PutAsync(string dbName);

        /// <summary>
        /// Creates the database, but only if it does not already exist.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> PutAsync(PutDatabaseRequest request);

        /// <summary>
        /// Deletes the database.
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> DeleteAsync(string dbName);

        /// <summary>
        /// Deletes the database.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> DeleteAsync(DeleteDatabaseRequest request);

        /// <summary>
        /// Requests compaction of the database.
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> CompactAsync(string dbName);

        /// <summary>
        /// Requests compaction of the database.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> CompactAsync(CompactDatabaseRequest request);

        /// <summary>
        /// Removes view index files that are no longer required by CouchDB as a
        /// result of changed views within design documents.
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> ViewCleanupAsync(string dbName);

        /// <summary>
        /// Removes view index files that are no longer required by CouchDB as a
        /// result of changed views within design documents.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> ViewCleanupAsync(ViewCleanupRequest request);

        /// <summary>
        /// Initiates a new Replication task.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        Task<ReplicationResponse> ReplicateAsync(string id, string source, string target);

        /// <summary>
        /// Initiates a new Replication task.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ReplicationResponse> ReplicateAsync(ReplicateDatabaseRequest request);
    }
}