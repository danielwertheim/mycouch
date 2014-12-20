using EnsureThat;
using System;

namespace MyCouch
{
    /// <summary>
    /// Used to identify a certain list function of a design document.
    /// </summary>
#if !PCL
    [Serializable]
#endif
    public class ListIdentity
    {
        public string DesignDocument { get; private set; }
        public string Name { get; private set; }

        public ListIdentity(string designDocument, string name)
        {
            Ensure.That(designDocument, "designDocument").IsNotNullOrWhiteSpace();
            Ensure.That(name, "name").IsNotNullOrWhiteSpace();

            DesignDocument = designDocument;
            Name = name;
        }
    }
}
