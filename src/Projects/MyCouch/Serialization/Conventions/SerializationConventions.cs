namespace MyCouch.Serialization.Conventions
{
    public class SerializationConventions
    {
        public ISerializationConvention DocType { get; set; }

        public SerializationConventions()
        {
            DocType = new DocTypeSerializationConvention();
        }
    }
}