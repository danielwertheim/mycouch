using System.Reflection;

namespace MyCouch.Schemes.Reflections
{
    public interface IDynamicPropertyFactory
    {
        DynamicProperty PropertyFor(PropertyInfo p);
    }
}