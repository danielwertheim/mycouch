using System;
using MyCouch.EntitySchemes;
using MyCouch.EntitySchemes.Reflections;
using MyCouch.Net;
using MyCouch.ResponseFactories;
using MyCouch.Serialization;

namespace MyCouch
{
    public class ClientBootsraper
    {
        public Func<SerializationConfiguration> SerializationConfigurationFn { get; set; }
        public Func<SerializationConfiguration> EntitySerializationConfigurationFn { get; set; }
        public Func<EntityReflector> EntityReflectorFn { get; set; }

        public Func<ISerializer> SerializerFn { get; set; }
        public Func<ISerializer> EntitySerializerFn { get; set; }
        public Func<IConnection, IAttachments> AttachmentsFn { get; set; }
        public Func<IConnection, IDatabases> DatabasesFn { get; set; }
        public Func<IConnection, IDocuments> DocumentsFn { get; set; }
        public Func<IConnection, IEntities> EntitiesFn { get; set; }
        public Func<IConnection, IViews> ViewsFn { get; set; }

        public ClientBootsraper()
        {
            ConfigureAttachmentsFn();
            ConfigureDatabasesFn();
            ConfigureDocumentsFn();
            ConfigureEntitiesFn();
            ConfigureViewsFn();

            ConfigureSerializationConfigurationFn();
            ConfigureEntitySerializationConfigurationFn();
         
            ConfigureSerializerFn();
            ConfigureEntitySerializerFn();
            ConfigureEntityReflectorFn();
        }

        private void ConfigureAttachmentsFn()
        {
            AttachmentsFn = cn =>
            {
                var responseMaterializer = new DefaultResponseMaterializer(SerializationConfigurationFn());
                
                return new Attachments(
                    cn,
                    new AttachmentResponseFactory(responseMaterializer),
                    new DocumentHeaderResponseFactory(responseMaterializer));
            };
        }

        private void ConfigureDatabasesFn()
        {
            DatabasesFn = cn =>
            {
                var responseMaterializer = new DefaultResponseMaterializer(SerializationConfigurationFn());
                
                return new Databases(
                    cn,
                    new DatabaseResponseFactory(responseMaterializer));
            };
        }

        private void ConfigureDocumentsFn()
        {
            DocumentsFn = cn =>
            {
                var responseMaterializer = new DefaultResponseMaterializer(SerializationConfigurationFn());

                return new Documents(
                    cn,
                    new DocumentResponseFactory(responseMaterializer),
                    new DocumentHeaderResponseFactory(responseMaterializer),
                    new BulkResponseFactory(responseMaterializer));
            };
        }

        private void ConfigureEntitiesFn()
        {
            EntitiesFn = cn =>
            {
                var entityReflector = EntityReflectorFn();
                var contractResolver = new EntitySerializationContractResolver(entityReflector);
                var serializationConfiguration = new SerializationConfiguration(contractResolver)
                {
                    WriterFactory = (t, w) => new EntityJsonWriter(t, w)
                };
                var responseMaterializer = new DefaultResponseMaterializer(serializationConfiguration);

                return new Entities(
                    cn,
                    new EntityResponseFactory(responseMaterializer, EntitySerializerFn()),
                    EntitySerializerFn(),
                    entityReflector);
            };
        }

        private void ConfigureViewsFn()
        {
            ViewsFn = cn =>
            {
                var contractResolver = new EntitySerializationContractResolver(EntityReflectorFn());
                var serializationConfiguration = new SerializationConfiguration(contractResolver);
                var responseMaterializer = new DefaultResponseMaterializer(serializationConfiguration);

                return new Views(
                    cn,
                    new JsonViewQueryResponseFactory(responseMaterializer),
                    new ViewQueryResponseFactory(responseMaterializer));
            };
        }

        private void ConfigureEntityReflectorFn()
        {
#if !NETFX_CORE
            var entityReflector = new Lazy<EntityReflector>(() => new EntityReflector(new IlDynamicPropertyFactory()));
#else
            var entityReflector = new Lazy<EntityReflector>(() => new EntityReflector(new LambdaDynamicPropertyFactory()));
#endif
            EntityReflectorFn = () => entityReflector.Value;
        }

        private void ConfigureSerializationConfigurationFn()
        {
            var serializationConfiguration = new Lazy<SerializationConfiguration>(() =>
            {
                var contractResolver = new SerializationContractResolver();

                return new SerializationConfiguration(contractResolver);
            });

            SerializationConfigurationFn = () => serializationConfiguration.Value;
        }

        private void ConfigureEntitySerializationConfigurationFn()
        {
            var serializationConfiguration = new Lazy<SerializationConfiguration>(() =>
            {
                var contractResolver = new EntitySerializationContractResolver(EntityReflectorFn());

                return new SerializationConfiguration(contractResolver)
                {
                    WriterFactory = (t, w) => new EntityJsonWriter(t, w)
                };
            });

            EntitySerializationConfigurationFn = () => serializationConfiguration.Value;
        }

        private void ConfigureSerializerFn()
        {
            var serializer = new Lazy<DefaultSerializer>(() => new DefaultSerializer(SerializationConfigurationFn()));
            SerializerFn = () => serializer.Value;
        }

        private void ConfigureEntitySerializerFn()
        {
            var serializer = new Lazy<DefaultSerializer>(() => new DefaultSerializer(EntitySerializationConfigurationFn()));
            EntitySerializerFn = () => serializer.Value;
        }
    }
}