using EnsureThat;

namespace MyCouch.Serialization.Conventions
{
    public class SerializationConventionWriter : ISerializationConventionWriter
    {
        protected readonly DocumentJsonWriter InnerWriter;

        public SerializationConventionWriter(DocumentJsonWriter jsonWriter)
        {
            EnsureArg.IsNotNull(jsonWriter, nameof(jsonWriter));

            InnerWriter = jsonWriter;
        }

        public virtual ISerializationConventionWriter WriteName(string name)
        {
            InnerWriter.WritePropertyName(name);

            return this;
        }

        public virtual ISerializationConventionWriter WriteValue(string value)
        {
            InnerWriter.WriteValue(value);

            return this;
        }
    }
}