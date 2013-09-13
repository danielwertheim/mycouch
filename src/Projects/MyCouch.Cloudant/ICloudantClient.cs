namespace MyCouch.Cloudant
{
    public interface ICloudantClient : IClient
    {
        ISearches Searches { get; } 
    }
}