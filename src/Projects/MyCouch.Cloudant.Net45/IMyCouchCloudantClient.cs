namespace MyCouch.Cloudant
{
    public interface IMyCouchCloudantClient : IMyCouchClient
    {
        ISearches Searches { get; }
    }
}