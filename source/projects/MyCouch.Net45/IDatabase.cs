using System.Threading.Tasks;
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
        Task<GetDatabaseResponse> GetAsync();

        /// <summary>
        /// Returns the HTTP Headers containing a minimal amount of information
        /// about the specified database. Since the response body is empty,
        /// using the HEAD method is a lightweight way to check if the database
        /// exists already or not.
        /// </summary>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> HeadAsync();

        /// <summary>
        /// Creates the database, but only if it does not already exist.
        /// </summary>
        Task<DatabaseHeaderResponse> PutAsync();

        /// <summary>
        /// Deletes the database.
        /// </summary>
        Task<DatabaseHeaderResponse> DeleteAsync();

        /// <summary>
        /// Requests compaction of the database.
        /// </summary>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> CompactAsync();

        /// <summary>
        /// Removes view index files that are no longer required by CouchDB as a
        /// result of changed views within design documents.
        /// </summary>
        /// <returns></returns>
        Task<DatabaseHeaderResponse> ViewCleanupAsync();
    }
}