namespace MyCouch.Requests.Factories
{
    public abstract class DocumentHttpRequestFactoryBase : HttpRequestFactoryBase
    {
        protected DocumentHttpRequestFactoryBase(IConnection connection) : base(connection) {}

        protected virtual string GenerateRequestUrl(string id = null, string rev = null)
        {
            return string.Format("{0}/{1}{2}",
                Connection.Address,
                id ?? string.Empty,
                rev == null ? string.Empty : string.Concat("?rev=", rev));
        }
    }
}