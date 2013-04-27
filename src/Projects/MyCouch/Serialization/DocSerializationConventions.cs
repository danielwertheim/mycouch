using MyCouch.Serialization.Conventions;

namespace MyCouch.Serialization
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