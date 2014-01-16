using MyCouch.Serialization.Meta;

namespace MyCouch.Serialization
{
    public class EntitySerializer : DocumentSerializer, IEntitySerializer
    {
        public EntitySerializer(SerializationConfiguration configuration, IDocumentSerializationMetaProvider documentSerializationMetaProvider)
            : base(configuration, documentSerializationMetaProvider)
        {
        }
    }
}