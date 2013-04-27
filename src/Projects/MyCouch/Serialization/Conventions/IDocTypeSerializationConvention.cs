using System;

namespace MyCouch.Serialization.Conventions
{
    public interface IDocTypeSerializationConvention
    {
        string PropertyName { get; }
        Func<Type, string> Convention { get; }
    }
}