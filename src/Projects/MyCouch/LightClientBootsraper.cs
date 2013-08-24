using System;
using MyCouch.Net;
using MyCouch.ResponseFactories;
using MyCouch.Serialization;

namespace MyCouch
{
    public class LightClientBootsraper
    {
        public Func<SerializationConfiguration> SerializationConfigurationResolver { get; set; }
        public Func<IResponseMaterializer> ResponseMaterializerResolver { get; set; }
        public Func<ISerializer> SerializerResolver { get; set; }
        public Func<IConnection, IAttachments> AttachmentsResolver { get; set; }
        public Func<IConnection, IDatabases> DatabasesResolver { get; set; }
        public Func<IConnection, IDocuments> DocumentsResolver { get; set; }
        public Func<IConnection, IViews> ViewsResolver { get; set; }

        public LightClientBootsraper()
        {
            ConfigureSerializationConfigurationResolver();
            ConfigureResponseMaterializerResolver();
            ConfigureSerializerResolver();
            ConfigureAttachmentsResolver();
            ConfigureDatabasesResolver();
            ConfigureDocumentsResolver();
            ConfigureViewsResolver();
        }

        private void ConfigureSerializationConfigurationResolver()
        {
            var serializationConfiguration = new Lazy<SerializationConfiguration>(() => new SerializationConfiguration(new SerializationContractResolver()));
            SerializationConfigurationResolver = () => serializationConfiguration.Value;
        }

        private void ConfigureResponseMaterializerResolver()
        {
            var materializer = new Lazy<DefaultResponseMaterializer>(() => new DefaultResponseMaterializer(SerializationConfigurationResolver()));
            ResponseMaterializerResolver = () => materializer.Value;
        }

        private void ConfigureSerializerResolver()
        {
            var serializer = new Lazy<DefaultSerializer>(() => new DefaultSerializer(SerializationConfigurationResolver()));
            SerializerResolver = () => serializer.Value;
        }

        private void ConfigureAttachmentsResolver()
        {
            AttachmentsResolver = cn => new Attachments(
                cn,
                new AttachmentResponseFactory(ResponseMaterializerResolver()),
                new DocumentHeaderResponseFactory(ResponseMaterializerResolver()));
        }

        private void ConfigureDatabasesResolver()
        {
            DatabasesResolver = cn => new Databases(
                cn, 
                new DatabaseResponseFactory(ResponseMaterializerResolver()));
        }

        private void ConfigureDocumentsResolver()
        {
            DocumentsResolver = cn => new Documents(
                cn, 
                new DocumentResponseFactory(ResponseMaterializerResolver()), 
                new DocumentHeaderResponseFactory(ResponseMaterializerResolver()), 
                new BulkResponseFactory(ResponseMaterializerResolver()));
        }

        private void ConfigureViewsResolver()
        {
            ViewsResolver = cn => new Views(
                cn,
                new JsonViewQueryResponseFactory(ResponseMaterializerResolver()),
                new ViewQueryResponseFactory(ResponseMaterializerResolver()));
        }
    }
}