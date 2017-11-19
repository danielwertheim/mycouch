using EnsureThat;

namespace MyCouch
{
    /// <summary>
    /// Used to identify a certain view of a design document.
    /// For system views like _all_docs, use <see cref="SystemViewIdentity"/>
    /// </summary>
    public class ViewIdentity
    {
        public string DesignDocument { get; private set; }
        public string Name { get; private set; }

        public ViewIdentity(string designDocument, string name)
        {
            EnsureArg.IsNotNullOrWhiteSpace(designDocument, nameof(designDocument));
            EnsureArg.IsNotNullOrWhiteSpace(name, nameof(name));

            DesignDocument = designDocument;
            Name = name;
        }

        protected ViewIdentity(string name)
        {
            EnsureArg.IsNotNullOrWhiteSpace(name, nameof(name));

            Name = name;
        }
    }
}