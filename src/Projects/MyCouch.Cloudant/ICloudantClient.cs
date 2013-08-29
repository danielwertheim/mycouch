namespace MyCouch.Cloudant
{
    public interface ICloudantClient : IClient
    {
        ISearch Search { get; } 
    }
}