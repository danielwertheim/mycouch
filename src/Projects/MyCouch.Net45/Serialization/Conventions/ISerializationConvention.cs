using System;
using MyCouch.Serialization.Meta;

namespace MyCouch.Serialization.Conventions
{
    public interface ISerializationConvention
    {
        string PropertyName { get; }
        Func<DocumentSerializationMeta, string> Convention { get; }
    }
}