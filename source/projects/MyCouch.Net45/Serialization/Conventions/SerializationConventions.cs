namespace MyCouch.Serialization.Conventions
{
    public class SerializationConventions
    {
        public ISerializationConvention DocType { get; set; }
        public ISerializationConvention DocNamespace { get; set; }
        public ISerializationConvention DocVersion { get; set; }

        public SerializationConventions()
        {
            DocType = new StringSerializationConvention(
                "$doctype",
                m => !m.IsAnonymous ? m.DocType.ToLowerInvariant() : null);

            DocNamespace = new StringSerializationConvention(
                "$docns",
                m => m.DocNamespace);

            DocVersion = new StringSerializationConvention(
                "$docver",
                m => m.DocVersion);
        }
    }
}