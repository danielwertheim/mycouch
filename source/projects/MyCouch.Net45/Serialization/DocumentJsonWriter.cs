using System.IO;
using EnsureThat;
using MyCouch.EntitySchemes;
using MyCouch.Serialization.Conventions;
using MyCouch.Serialization.Meta;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    public class DocumentJsonWriter : JsonTextWriter
    {
        private const string CouchIdMemberName = "_id";
        private const string CouchRevMemberName = "_rev";

        protected readonly DocumentSerializationMeta DocumentMeta;
        protected readonly SerializationConventions Conventions;
        protected readonly ISerializationConventionWriter ConventionWriter;
        protected readonly IEntityReflector EntityReflector;

        protected bool HasWrittenDocumentMeta { get; private set; }
        protected int Level { get; private set; }

        public DocumentJsonWriter(TextWriter textWriter, DocumentSerializationMeta documentMeta, SerializationConventions conventions, IEntityReflector entityReflector)
            : base(textWriter)
        {
            Ensure.That(documentMeta, "documentMeta").IsNotNull();
            Ensure.That(conventions, "conventions").IsNotNull();
            Ensure.That(conventions, "entityReflector").IsNotNull();

            HasWrittenDocumentMeta = false;
            DocumentMeta = documentMeta;
            Conventions = conventions;
            ConventionWriter = new SerializationConventionWriter(this);
            EntityReflector = entityReflector;
            CloseOutput = false;
        }

        public override void WriteStartObject()
        {
            Level += 1;

            base.WriteStartObject();

            if (Level > 1 || HasWrittenDocumentMeta || !Conventions.HasConventions)
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

        public override void WriteEndObject()
        {
            Level -= 1;

            base.WriteEndObject();
        }

        public override void WriteNull()
        {
            base.WriteRaw(string.Empty);
        }

        public override void WritePropertyName(string name)
        {
            if (Level > 1 || name == CouchIdMemberName || name == CouchRevMemberName)
            {
                base.WritePropertyName(name);
                return;
            }

            var idIndex = EntityReflector.IdMember.GetMemberRankingIndex(DocumentMeta.Type, name);
            if (idIndex != null)
            {
                base.WritePropertyName(CouchIdMemberName);
                return;
            }

            var revIndex = EntityReflector.RevMember.GetMemberRankingIndex(DocumentMeta.Type, name);
            if (revIndex != null)
            {
                base.WritePropertyName(CouchRevMemberName);
                return;
            }

            base.WritePropertyName(name);
        }

        public override void WritePropertyName(string name, bool escape)
        {
            if (Level > 1 || name == CouchIdMemberName || name == CouchRevMemberName)
            {
                base.WritePropertyName(name, escape);
                return;
            }

            var idIndex = EntityReflector.IdMember.GetMemberRankingIndex(DocumentMeta.Type, name);
            if (idIndex != null)
            {
                base.WritePropertyName(CouchIdMemberName, escape);
                return;
            }

            var revIndex = EntityReflector.RevMember.GetMemberRankingIndex(DocumentMeta.Type, name);
            if (revIndex != null)
            {
                base.WritePropertyName(CouchRevMemberName, escape);
                return;
            }

            base.WritePropertyName(name, escape);
        }
    }
}