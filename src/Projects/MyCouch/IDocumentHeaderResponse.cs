namespace MyCouch
{
    public interface IDocumentHeaderResponse : IResponse
    {
        string Id { get; set; }
        string Rev { get; set; }
    }
}