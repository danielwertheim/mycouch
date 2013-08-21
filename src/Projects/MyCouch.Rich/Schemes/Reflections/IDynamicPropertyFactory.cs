using System.Reflection;

namespace MyCouch.Rich.Schemes.Reflections
{
    public interface IDynamicPropertyFactory
    {
        DynamicProperty PropertyFor(PropertyInfo p);
    }
}