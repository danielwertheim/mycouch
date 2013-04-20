using System.Threading.Tasks;

namespace MyCouch
{
    /// <summary>
    /// Used to manage databases.
    /// </summary>
    public interface IDatabases
    {
        /// <summary>
        /// Creates a new database, named according to <paramref name="dbname"/>, if it does not allready exist.
        /// </summary>
        /// <param name="dbname"></param>
        DatabaseResponse Put(string dbname);

        /// <summary>
        /// Creates a new database, named according to <paramref name="dbname"/>, if it does not allready exist.
        /// </summary>
        /// <param name="dbname"></param>
        Task<DatabaseResponse> PutAsync(string dbname);

        /// <summary>
        /// Deletes an existing database matching the sent <paramref name="dbname"/>.
        /// </summary>
        /// <param name="dbname"></param>
        DatabaseResponse Delete(string dbname);

        /// <summary>
        /// Deletes an existing database matching the sent <paramref name="dbname"/>.
        /// </summary>
        /// <param name="dbname"></param>
        Task<DatabaseResponse> DeleteAsync(string dbname);
    }
}