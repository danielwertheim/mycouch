namespace MyCouch.Cloudant
{
    public interface IMyCouchCloudantClient : IMyCouchClient
    {
        /// <summary>
        /// Used to access Search Indexes at Cloudant.
        /// </summary>
        ISearches Searches { get; }

        /// <summary>
        /// Used to access the Query API at Cloudant, inspired by MongoDB syntax.
        /// </summary>
        IQueries Queries { get; }
    }
}