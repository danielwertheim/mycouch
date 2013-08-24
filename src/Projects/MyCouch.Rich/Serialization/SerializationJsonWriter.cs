using System;
using System.IO;
using EnsureThat;
using MyCouch.Rich.Serialization.Conventions;
using Newtonsoft.Json;

namespace MyCouch.Rich.Serialization
{
    public class SerializationJsonWriter : JsonTextWriter
    {
        protected readonly Type DocType;
        protected bool HasWrittenDocHeader = false;

        public SerializationConventions Conventions { get; protected set; }

        public SerializationJsonWriter(Type docType, TextWriter textWriter)
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