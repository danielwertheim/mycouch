using System;

namespace MyCouch.Serialization.Conventions
{
    public class DocTypeSerializationConvention : ISerializationConvention
    {
        public string PropertyName
        {
            get { return "$doctype"; }
        }

        public Func<Type, string> Convention { get; private set; }

        public DocTypeSerializationConvention()
        {
            Convention = t => t.Name.ToLowerInvariant();
        }
    }
}
