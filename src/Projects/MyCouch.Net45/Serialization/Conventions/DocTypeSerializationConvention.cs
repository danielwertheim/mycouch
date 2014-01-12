using System;
using MyCouch.Serialization.Meta;

namespace MyCouch.Serialization.Conventions
{
    public class DocTypeSerializationConvention : ISerializationConvention
    {
        public string PropertyName
        {
            get { return "$doctype"; }
        }

        public Func<DocumentSerializationMeta, string> Convention { get; private set; }

        public DocTypeSerializationConvention()
        {
            Convention = m => !m.IsAnonymous
                ? m.DocType.ToLowerInvariant()
                : string.Empty;
        }
    }
}