using EnsureThat;

namespace MyCouch
{
    /// <summary>
    /// Used to identify a certain search index in a design document.
    /// </summary>
    public class SearchIndexIdentity
    {
        public string DesignDocument { get; private set; }
        public string Name { get; private set; }

        public SearchIndexIdentity(string designDocument, string name)
        {
            EnsureArg.IsNotNullOrWhiteSpace(designDocument, nameof(designDocument));
            EnsureArg.IsNotNullOrWhiteSpace(name, nameof(name));

            DesignDocument = designDocument;
            Name = name;
        }
    }
}