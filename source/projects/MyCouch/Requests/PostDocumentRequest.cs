using EnsureThat;

namespace MyCouch.Requests
{
    public class PostDocumentRequest : Request
    {
        public bool Batch { get; set; }
        public string Content { get; set; }

        public PostDocumentRequest(string content)
        {
            EnsureArg.IsNotNullOrWhiteSpace(content, nameof(content));

            Batch = false;
            Content = content;
        }
    }
}