using System;
using MyCouch.EnsureThat;

namespace MyCouch.Cloudant
{
    /// <summary>
    /// Used to identify a certain search index in a design document.
    /// </summary>
#if !PCL && !vNext
    [Serializable]
#endif
    public class SearchIndexIdentity
    {
        public string DesignDocument { get; private set; }
        public string Name { get; private set; }

        public SearchIndexIdentity(string designDocument, string name)
        {
            Ensure.That(designDocument, "designDocument").IsNotNullOrWhiteSpace();
            Ensure.That(name, "name").IsNotNullOrWhiteSpace();

            DesignDocument = designDocument;
            Name = name;
        }
    }
}