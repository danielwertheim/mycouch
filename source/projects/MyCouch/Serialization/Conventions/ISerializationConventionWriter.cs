namespace MyCouch.Serialization.Conventions
{
    public interface ISerializationConventionWriter
    {
        ISerializationConventionWriter WriteName(string name);
        ISerializationConventionWriter WriteValue(string value);
    }
}