namespace MyCouch
{
    public class DocumentHeader : IDocumentHeader
    {
        public string Id { get; }
        public string Rev { get; }

        public DocumentHeader(string id, string rev)
        {
            Id = id;
            Rev = rev;
        }
    }
}