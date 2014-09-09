namespace MyCouch.Cloudant
{
    public interface IMyCouchCloudantClient : IMyCouchClient
    {
        /// <summary>
        /// Used to access Search Indexes at Cloudant.
        /// </summary>
        ISearches Searches { get; }
        IQueries Queries { get; }
    }
}