using System;
using EnsureThat;

namespace MyCouch
{
    /// <summary>
    /// Used to identify a certain view of a design document.
    /// For system views like _all_docs, use <see cref="SystemViewIdentity"/>
    /// </summary>
#if !NETFX_CORE
    [Serializable]
#endif
    public class ViewIdentity
    {
        public string DesignDocument { get; private set; }
        public string Name { get; private set; }

        public ViewIdentity(string designDocument, string name)
        {
            Ensure.That(designDocument, "designDocument").IsNotNullOrWhiteSpace();
            Ensure.That(name, "name").IsNotNullOrWhiteSpace();

            DesignDocument = designDocument;
            Name = name;
        }

        protected ViewIdentity(string name)
        {
            Ensure.That(name, "name").IsNotNullOrWhiteSpace();

            Name = name;
        }
    }
}