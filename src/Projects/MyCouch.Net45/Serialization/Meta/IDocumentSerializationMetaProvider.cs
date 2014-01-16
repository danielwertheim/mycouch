using System;

namespace MyCouch.Serialization.Meta
{
    public interface IDocumentSerializationMetaProvider
    {
        DocumentSerializationMeta Get(Type docType);
    }
}