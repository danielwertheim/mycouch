using System;
using MyCouch.Contexts;
using MyCouch.EntitySchemes;
using MyCouch.EntitySchemes.Reflections;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch
{
    public class ClientBootsraper
    {
        public Func<SerializationConfiguration> SerializationConfigurationFn { get; set; }
        public Func<SerializationConfiguration> EntitySerializationConfigurationFn { get; set; }
        public Func<IEntityReflector> EntityReflectorFn { get; set; }

        public Func<ISerializer> SerializerFn { get; set; }
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
            ConfigureEntityReflectorFn();
        }

        private void ConfigureAttachmentsFn()
        {
            AttachmentsFn = cn => new Attachments(cn, SerializationConfigurationFn());
        }

        private void ConfigureDatabasesFn()
        {
            DatabasesFn = cn => new Databases(cn, SerializationConfigurationFn());
        }

        private void ConfigureDocumentsFn()
        {
            DocumentsFn = cn => new Documents(cn, SerializationConfigurationFn());
        }

        private void ConfigureEntitiesFn()
        {
            EntitiesFn = cn => new Entities(
                cn,
                EntitySerializationConfigurationFn(),
                EntityReflectorFn());
        }

        private void ConfigureViewsFn()
        {
            ViewsFn = cn => new Views(
                cn,
                EntitySerializationConfigurationFn());
        }

        private void ConfigureEntityReflectorFn()
        {
#if !NETFX_CORE
            var entityReflector = new Lazy<IEntityReflector>(() => new EntityReflector(new IlDynamicPropertyFactory()));
#else
            var entityReflector = new Lazy<IEntityReflector>(() => new EntityReflector(new LambdaDynamicPropertyFactory()));
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
                var contractResolver = new EntityContractResolver(EntityReflectorFn());

                return new SerializationConfiguration(contractResolver);
            });

            EntitySerializationConfigurationFn = () => serializationConfiguration.Value;
        }

        private void ConfigureSerializerFn()
        {
            var serializer = new Lazy<DefaultSerializer>(() => new DefaultSerializer(SerializationConfigurationFn()));
            SerializerFn = () => serializer.Value;
        }
    }
}