namespace MyCouch.Responses
{
    public interface IContainDocumentHeader
    {
        string Id { get; set; }
        string Rev { get; set; }
    }
}