using System;
using System.IO;
using EnsureThat;
using MyCouch.Serialization.Conventions;

namespace MyCouch.Serialization.Writers
{
    /// <summary>
    /// When serializing entities, this writer will use
    /// conventions found in <see cref="Conventions"/>,
    /// which by default will e.g. inject info about doctype.
    /// </summary>
    public class EntityJsonWriter : MyCouchJsonWriter
    {
        protected readonly Type DocType;
        protected bool HasWrittenDocHeader = false;

        public SerializationConventions Conventions { get; protected set; }

        public EntityJsonWriter(Type docType, TextWriter textWriter)
            : base(textWriter)
        {
            Ensure.That(docType, "docType").IsNotNull();

            DocType = docType;
            Conventions = new SerializationConventions();
        }

        public override void WriteStartObject()
        {
            base.WriteStartObject();

            if (HasWrittenDocHeader) return;
            HasWrittenDocHeader = true;
            WriteDocType(DocType);
        }

        protected virtual void WriteDocType(Type docType)
        {
            WritePropertyName(Conventions.DocType.PropertyName);
            WriteValue(Conventions.DocType.Convention(docType));
        }
    }
}