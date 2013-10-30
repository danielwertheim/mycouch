using System;
using MyCouch.Contexts;
using MyCouch.EntitySchemes;
using MyCouch.EntitySchemes.Reflections;
using MyCouch.Serialization;

namespace MyCouch
{
    public class ClientBootstraper
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
        /// Used e.g. for bootstraping <see cref="IEntities.Serializer"/>.
        /// </summary>
        public Func<IEntitySerializer> EntitySerializerFn { get; set; }
        /// <summary>
        /// Used e.g. for bootstraping <see cref="IClient.Changes"/>.
        /// </summary>
        public Func<IConnection, IChanges> ChangesFn { get; set; }
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

        public ClientBootstraper()
        {
            ConfigureChangesFn();
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

        protected virtual void ConfigureChangesFn()
        {
            ChangesFn = cn => new Changes(cn, SerializerFn());
        }

        protected virtual void ConfigureAttachmentsFn()
        {
            AttachmentsFn = cn => new Attachments(cn, SerializerFn());
        }

        protected virtual void ConfigureDatabasesFn()
        {
            DatabasesFn = cn => new Databases(cn, SerializerFn());
        }

        protected virtual void ConfigureDocumentsFn()
        {
            DocumentsFn = cn => new Documents(cn, SerializerFn());
        }

        protected virtual void ConfigureEntitiesFn()
        {
            EntitiesFn = cn => new Entities(
                cn,
                SerializerFn(),
                EntitySerializerFn(),
                EntityReflectorFn());
        }

        protected virtual void ConfigureViewsFn()
        {
            ViewsFn = cn => new Views(
                cn,
                SerializerFn(),
                EntitySerializerFn());
        }

        protected virtual void ConfigureEntityReflectorFn()
        {
#if !NETFX_CORE
            var entityReflector = new Lazy<IEntityReflector>(() => new EntityReflector(new IlDynamicPropertyFactory()));
#else
            var entityReflector = new Lazy<IEntityReflector>(() => new EntityReflector(new LambdaDynamicPropertyFactory()));
#endif
            EntityReflectorFn = () => entityReflector.Value;
        }

        protected virtual void ConfigureSerializationConfigurationFn()
        {
            var serializationConfiguration = new Lazy<SerializationConfiguration>(() =>
            {
                var contractResolver = new SerializationContractResolver();

                return new SerializationConfiguration(contractResolver);
            });

            SerializationConfigurationFn = () => serializationConfiguration.Value;
        }

        protected virtual void ConfigureEntitySerializationConfigurationFn()
        {
            var serializationConfiguration = new Lazy<SerializationConfiguration>(() =>
            {
                var contractResolver = new EntityContractResolver(EntityReflectorFn());

                return new SerializationConfiguration(contractResolver);
            });

            EntitySerializationConfigurationFn = () => serializationConfiguration.Value;
        }

        protected virtual void ConfigureSerializerFn()
        {
            var serializer = new Lazy<ISerializer>(() => new DefaultSerializer(SerializationConfigurationFn()));
            SerializerFn = () => serializer.Value;
        }

        protected virtual void ConfigureEntitySerializerFn()
        {
            var serializer = new Lazy<IEntitySerializer>(() => new EntitySerializer(EntitySerializationConfigurationFn()));
            EntitySerializerFn = () => serializer.Value;
        }
    }
}