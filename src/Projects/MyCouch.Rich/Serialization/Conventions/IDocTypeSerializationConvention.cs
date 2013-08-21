using System;

namespace MyCouch.Rich.Serialization.Conventions
{
    public interface IDocTypeSerializationConvention
    {
        string PropertyName { get; }
        Func<Type, string> Convention { get; }
    }
}