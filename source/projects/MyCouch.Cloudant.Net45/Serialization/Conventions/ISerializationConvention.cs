using MyCouch.Serialization.Meta;

namespace MyCouch.Serialization.Conventions
{
    public interface ISerializationConvention
    {
        void Apply(DocumentSerializationMeta meta, ISerializationConventionWriter writer);
    }
}