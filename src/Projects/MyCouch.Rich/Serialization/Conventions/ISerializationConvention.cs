using System;

namespace MyCouch.Rich.Serialization.Conventions
{
    public interface ISerializationConvention
    {
        string PropertyName { get; }
        Func<Type, string> Convention { get; }
    }
}