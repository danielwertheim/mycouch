using System;
using EnsureThat;

namespace MyCouch.Serialization.Meta
{
#if !PCL
    [Serializable]
#endif
    public class DocumentSerializationMeta
    {
        public Type Type { get; private set; }
        public string DocType { get; private set; }
        public string DocNamespace { get; set; }
        public string DocVersion { get; set; }
        public bool IsAnonymous { get; private set; }

        public DocumentSerializationMeta(Type type, string docType, bool isAnonymous)
        {
            Ensure.That(type, "type").IsNotNull();
            Ensure.That(docType, "docType").IsNotNull();

            Type = type;
            DocType = docType;
            IsAnonymous = isAnonymous;
        }
    }
}