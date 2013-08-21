namespace MyCouch.Rich.Serialization.Conventions
{
    public class DocSerializationConventions
    {
        public IDocTypeSerializationConvention DocType { get; set; }

        public DocSerializationConventions()
        {
            DocType = new DocTypeSerializationConvention();
        }
    }
}