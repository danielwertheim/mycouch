using System.IO;
using EnsureThat;
using MyCouch.Serialization.Meta;
using MyCouch.Serialization.Writers;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    public class DocumentSerializer : DefaultSerializer, IDocumentSerializer
    {
        protected readonly IDocumentSerializationMetaProvider DocumentMetaProvider;

        public DocumentSerializer(SerializationConfiguration configuration, IDocumentSerializationMetaProvider documentSerializationMetaProvider)
            : base(configuration)
        {
            Ensure.That(documentSerializationMetaProvider, "documentSerializationMetaProvider").IsNotNull();

            DocumentMetaProvider = documentSerializationMetaProvider;
        }

        protected override JsonTextWriter CreateWriterFor<T>(TextWriter writer)
        {
            return new DocumentJsonWriter(DocumentMetaProvider.Get(typeof(T)), writer, Configuration.Conventions);
        }
    }
}