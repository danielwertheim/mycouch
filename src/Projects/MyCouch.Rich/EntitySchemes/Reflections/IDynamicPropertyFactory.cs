using System.Reflection;

namespace MyCouch.Rich.EntitySchemes.Reflections
{
    public interface IDynamicPropertyFactory
    {
        DynamicProperty PropertyFor(PropertyInfo p);
    }
}