using System;
using MyCouch.Contexts;
using MyCouch.EntitySchemes;
using MyCouch.EntitySchemes.Reflections;
using MyCouch.Serialization;

namespace MyCouch
{
    public class ClientBootsraper
    {
        /// <summary>
        /// Used e.g. for bootstraping components relying on serialization, e.g <see cref="ISerializer"/>
        /// used in <see cref="IClient.Serializer"/>.
        /// </summary>
        /// <remarks>For entity serialization configuration, <see cref="EntitySerializationConfigurationFn"/>.</remarks>
        public Func<SerializationConfiguration> SerializationConfigurationFn { get; set; }
        /// <summary>
        /// Used e.g. for bootstraping components relying on entity serialization, e.g <see cref="ISerializer"/>
        /// used in <see cref="IEntities.Serializer"/> used in <see cref="IClient.Entities"/>.
        /// </summary>
        public Func<SerializationConfiguration> EntitySerializationConfigurationFn { get; set; }
        /// <summary>
        /// Used e.g. for boostraping components that needs to be able to read and set values
        /// effectively to entities. Used e.g. in <see cref="IEntities.Reflector"/>.
        /// </summary>
        public Func<IEntityReflector> EntityReflectorFn { get; set; }
        /// <summary>
        /// Used e.g. for bootstraping <see cref="IClient.Serializer"/>.
        /// </summary>
        public Func<ISerializer> SerializerFn { get; set; }
        /// <summary>
        /// Used e.g. for bootstraping <see cref="IClient.Attachments"/>.
        /// </summary>
        public Func<IConnection, IAttachments> AttachmentsFn { get; set; }
        /// <summary>
        /// Used e.g. for bootstraping <see cref="IClient.Databases"/>.
        /// </summary>
        public Func<IConnection, IDatabases> DatabasesFn { get; set; }
        /// <summary>
        /// Used e.g. for bootstraping <see cref="IClient.Documents"/>.
        /// </summary>
        public Func<IConnection, IDocuments> DocumentsFn { get; set; }
        /// <summary>
        /// Used e.g. for bootstraping <see cref="IClient.Entities"/>.
        /// </summary>
        public Func<IConnection, IEntities> EntitiesFn { get; set; }
        /// <summary>
        /// Used e.g. for bootstraping <see cref="IClient.Views"/>.
        /// </summary>
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