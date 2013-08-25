using System;

namespace MyCouch.Serialization.Conventions
{
    public interface ISerializationConvention
    {
        string PropertyName { get; }
        Func<Type, string> Convention { get; }
    }
}