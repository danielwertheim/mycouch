using System.IO;
using EnsureThat;
using MyCouch.Serialization.Conventions;
using MyCouch.Serialization.Meta;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    public class DocumentJsonWriter : JsonTextWriter
    {
        protected readonly DocumentSerializationMeta DocumentMeta;
        protected readonly SerializationConventions Conventions;
        protected readonly ISerializationConventionWriter ConventionWriter;
        protected bool HasWrittenDocumentMeta { get; set; }

        public DocumentJsonWriter(DocumentSerializationMeta documentMeta, TextWriter textWriter, SerializationConventions conventions)
            : base(textWriter)
        {
            Ensure.That(documentMeta, "documentMeta").IsNotNull();
            Ensure.That(conventions, "conventions").IsNotNull();

            HasWrittenDocumentMeta = false;
            DocumentMeta = documentMeta;
            Conventions = conventions;
            ConventionWriter = new SerializationConventionWriter(this);
        }

        public override void WriteStartObject()
        {
            base.WriteStartObject();

            if (HasWrittenDocumentMeta || !Conventions.HasConventions)
                return;

            HasWrittenDocumentMeta = true;

            WriteDocumentMeta(DocumentMeta);
        }

        protected virtual void WriteDocumentMeta(DocumentSerializationMeta meta)
        {
            WriteDocumentMetaConvention(Conventions.DocType, meta);
            WriteDocumentMetaConvention(Conventions.DocNamespace, meta);
            WriteDocumentMetaConvention(Conventions.DocVersion, meta);
        }

        protected virtual void WriteDocumentMetaConvention(ISerializationConvention convention, DocumentSerializationMeta meta)
        {
            if(convention == null)
                return;

            convention.Apply(meta, ConventionWriter);
        }

        public override void WriteNull()
        {
            base.WriteRaw(string.Empty);
        }
    }
}