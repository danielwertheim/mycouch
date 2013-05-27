using System.Threading.Tasks;

namespace MyCouch
{
    /// <summary>
    /// Used to manage a database.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Creates the database, but only if it does not already exist.
        /// </summary>
        DatabaseResponse Put();

        /// <summary>
        /// Creates the database, but only if it does not already exist.
        /// </summary>
        Task<DatabaseResponse> PutAsync();

        /// <summary>
        /// Deletes the database.
        /// </summary>
        DatabaseResponse Delete();

        /// <summary>
        /// Deletes the database.
        /// </summary>
        Task<DatabaseResponse> DeleteAsync();
    }
}