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
        /// <param name="request"></param>
        /// <returns></returns>
        Task<TextResponse> GetAsync(GetDatabaseRequest request);

        /// <summary>
        /// Returns the HTTP Headers containing a minimal amount of information
        /// about the specified database. Since the response body is empty,
        /// using the HEAD method is a lightweight way to check if the database
        /// exists already or not.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<TextResponse> HeadAsync(HeadDatabaseRequest request);

        /// <summary>
        /// Creates the database, but only if it does not already exist.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<TextResponse> PutAsync(PutDatabaseRequest request);

        /// <summary>
        /// Deletes the database.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<TextResponse> DeleteAsync(DeleteDatabaseRequest request);

        /// <summary>
        /// Requests compaction of the database.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<TextResponse> CompactAsync(CompactDatabaseRequest request);

        /// <summary>
        /// Removes view index files that are no longer required by CouchDB as a
        /// result of changed views within design documents.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<TextResponse> ViewCleanup(ViewCleanupRequest request);
    }
}