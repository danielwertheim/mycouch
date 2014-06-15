namespace MyCouch
{
    public interface IDocumentHeader
    {
        string Id { get; }
        string Rev { get; }
    }
}