using System;

namespace MyCouch.Serialization.Meta
{
    public class EmptyDocumentSerializationMetaProvider : IDocumentSerializationMetaProvider
    {
        public DocumentSerializationMeta Get(Type docType)
        {
            return null;
        }
    }
}