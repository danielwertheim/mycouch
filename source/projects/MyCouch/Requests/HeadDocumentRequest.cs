using EnsureThat;

namespace MyCouch.Requests
{
    public class HeadDocumentRequest : Request
    {
        public string Id { get; private set; }
        public string Rev { get; private set; }

        public HeadDocumentRequest(string id, string rev = null)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();

            Id = id;
            Rev = rev;
        }
    }
}