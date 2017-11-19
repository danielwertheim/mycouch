using EnsureThat;

namespace MyCouch.Requests
{
    public class DeleteDocumentRequest : Request
    {
        public string Id { get; private set; }
        public string Rev { get; private set; }

        public DeleteDocumentRequest(string id, string rev)
        {
            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));
            EnsureArg.IsNotNullOrWhiteSpace(rev, nameof(rev));

            Id = id;
            Rev = rev;
        }
    }
}