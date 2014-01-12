using System;
using EnsureThat;

namespace MyCouch.Serialization.Meta
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class DocumentSerializationMeta
    {
        public string DocType { get; private set; }
        public string Namespace { get; private set; }
        public bool IsAnonymous { get; private set; }

        public DocumentSerializationMeta(string docType, string @namespace, bool isAnonymous)
        {
            Ensure.That(docType, "docType").IsNotNull();

            DocType = docType;
            Namespace = @namespace;
            IsAnonymous = isAnonymous;
        }
    }
}