namespace MyCouch
{
    public interface IDbClientConnection : IConnection
    {
        string DbName { get; }
    }
}