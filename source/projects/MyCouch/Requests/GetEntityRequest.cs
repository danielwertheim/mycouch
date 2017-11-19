using EnsureThat;

namespace MyCouch.Requests
{
    public class GetEntityRequest : Request
    {
        public string Id { get; private set; }
        public string Rev { get; private set; }
        public bool? Conflicts { get; set; }

        public GetEntityRequest(string id, string rev = null)
        {
            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));

            Id = id;
            Rev = rev;
        }
    }
}