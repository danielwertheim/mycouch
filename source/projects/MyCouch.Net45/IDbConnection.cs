namespace MyCouch
{
    public interface IDbConnection : IConnection
    {
        string DbName { get; }
    }
}