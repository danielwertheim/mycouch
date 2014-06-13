namespace MyCouch
{
    public interface IDocumentHeader
    {
        string Id { get; set; }
        string Rev { get; set; }
    }
}