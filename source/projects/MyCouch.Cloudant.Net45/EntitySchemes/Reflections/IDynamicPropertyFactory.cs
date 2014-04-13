using System.Reflection;

namespace MyCouch.EntitySchemes.Reflections
{
    public interface IDynamicPropertyFactory
    {
        DynamicProperty PropertyFor(PropertyInfo p);
    }
}