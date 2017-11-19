using EnsureThat;

namespace MyCouch.Requests
{
    public class GetDocumentRequest : Request
    {
        public string Id { get; private set; }
        public string Rev { get; private set; }
        public bool? Conflicts { get; set; }

        public GetDocumentRequest(string id, string rev = null)
        {
            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));

            Id = id;
            Rev = rev;
        }
    }
}