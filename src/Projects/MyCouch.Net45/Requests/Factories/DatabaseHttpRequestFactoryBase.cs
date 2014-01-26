namespace MyCouch.Requests.Factories
{
    public abstract class DatabaseHttpRequestFactoryBase : HttpRequestFactoryBase
    {
        protected DatabaseHttpRequestFactoryBase(IConnection connection) : base(connection) { }

        protected virtual string GenerateRequestUrl()
        {
            return Connection.Address.ToString();
        }
    }
}