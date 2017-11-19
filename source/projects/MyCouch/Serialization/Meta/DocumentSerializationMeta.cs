using System;
using EnsureThat;

namespace MyCouch.Serialization.Meta
{
    public class DocumentSerializationMeta
    {
        public Type Type { get; private set; }
        public string DocType { get; private set; }
        public string DocNamespace { get; set; }
        public string DocVersion { get; set; }
        public bool IsAnonymous { get; private set; }

        public DocumentSerializationMeta(Type type, string docType, bool isAnonymous)
        {
            EnsureArg.IsNotNull(type, nameof(type));
            EnsureArg.IsNotNull(docType, nameof(docType));

            Type = type;
            DocType = docType;
            IsAnonymous = isAnonymous;
        }
    }
}