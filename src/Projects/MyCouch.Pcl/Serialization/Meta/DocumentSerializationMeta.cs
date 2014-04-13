using System;
using MyCouch.EnsureThat;

namespace MyCouch.Serialization.Meta
{
#if !PCL
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