using EnsureThat;

namespace MyCouch
{
    public class ShowIdentity
    {
        public string DesignDocument { get; private set; }
        public string Name { get; private set; }

        public ShowIdentity(string designDocument, string name)
        {
            EnsureArg.IsNotNullOrWhiteSpace(designDocument, nameof(designDocument));
            EnsureArg.IsNotNullOrWhiteSpace(name, nameof(name));

            DesignDocument = designDocument;
            Name = name;
        }
    }
}