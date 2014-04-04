namespace MyCouch.Net
{
    public interface IDbRequestUrlGenerator
    {
        string Generate(string dbName);
    }
}