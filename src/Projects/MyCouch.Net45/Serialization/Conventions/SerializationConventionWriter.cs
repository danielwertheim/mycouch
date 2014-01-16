using EnsureThat;
using MyCouch.Serialization.Writers;

namespace MyCouch.Serialization.Conventions
{
    public class SerializationConventionWriter : ISerializationConventionWriter
    {
        protected readonly DocumentJsonWriter InnerWriter;

        public SerializationConventionWriter(DocumentJsonWriter jsonWriter)
        {
            Ensure.That(jsonWriter, "jsonWriter").IsNotNull();

            InnerWriter = jsonWriter;
        }

        public ISerializationConventionWriter WriteName(string name)
        {
            InnerWriter.WritePropertyName(name);

            return this;
        }

        public ISerializationConventionWriter WriteValue(string value)
        {
            InnerWriter.WriteValue(value);

            return this;
        }
    }
}