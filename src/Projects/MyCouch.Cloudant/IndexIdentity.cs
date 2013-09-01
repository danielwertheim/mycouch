using System;
using EnsureThat;

namespace MyCouch.Cloudant
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class IndexIdentity
    {
        public string DesignDocument { get; private set; }
        public string Name { get; private set; }

        public IndexIdentity(string designDocument, string name)
        {
            Ensure.That(designDocument, "designDocument").IsNotNullOrWhiteSpace();
            Ensure.That(name, "name").IsNotNullOrWhiteSpace();

            DesignDocument = designDocument;
            Name = name;
        }
    }
}