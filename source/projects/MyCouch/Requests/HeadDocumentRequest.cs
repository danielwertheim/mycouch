using EnsureThat;

namespace MyCouch.Requests
{
    public class HeadDocumentRequest : Request
    {
        public string Id { get; private set; }
        public string Rev { get; private set; }

        public HeadDocumentRequest(string id, string rev = null)
        {
            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));

            Id = id;
            Rev = rev;
        }
    }
}