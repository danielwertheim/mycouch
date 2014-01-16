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
        public string DocNamespace { get; set; }
        public string DocVersion { get; set; }
        public bool IsAnonymous { get; private set; }

        public DocumentSerializationMeta(string docType, bool isAnonymous)
        {
            Ensure.That(docType, "docType").IsNotNull();

            DocType = docType;
            IsAnonymous = isAnonymous;
        }
    }
}