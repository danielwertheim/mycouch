namespace MyCouch.Serialization
{
    public class EntitySerializer : DefaultSerializer, IEntitySerializer
    {

        public EntitySerializer(SerializationConfiguration configuration)
            : base(configuration)
        {
        }
    }
}