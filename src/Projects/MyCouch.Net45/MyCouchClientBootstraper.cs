using System;
using MyCouch.Contexts;
using MyCouch.EntitySchemes;
using MyCouch.EntitySchemes.Reflections;
using MyCouch.Serialization;
using MyCouch.Serialization.Meta;

namespace MyCouch
{
    public class MyCouchClientBootstraper
    {
        /// <summary>
        /// Used e.g. for bootstraping components relying on plain serialization, <see cref="ISerializer"/>
        /// used in <see cref="IMyCouchClient.Serializer"/>.
        /// </summary>
        /// <remarks>
        /// For document serialization configuration, <see cref="DocumentSerializationConfigurationFn"/>
        /// For entity serialization configuration, <see cref="EntitySerializationConfigurationFn"/>.
        /// </remarks>
        public Func<SerializationConfiguration> SerializationConfigurationFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping components relying on document serialization, <see cref="IDocumentSerializer"/>
        /// used in <see cref="IDocuments.Serializer"/> used in <see cref="IMyCouchClient.Documents"/>.
        /// </summary>
        /// <remarks>
        /// For plain serialization configuration, <see cref="SerializationConfigurationFn"/>.
        /// For entity serialization configuration, <see cref="EntitySerializationConfigurationFn"/>.
        /// </remarks>
        public Func<SerializationConfiguration> DocumentSerializationConfigurationFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping components relying on entity serialization, <see cref="IEntitySerializer"/>
        /// used in <see cref="IEntities.Serializer"/> used in <see cref="IMyCouchClient.Entities"/>.
        /// </summary>
        /// <remarks>
        /// For plain serialization configuration, <see cref="SerializationConfigurationFn"/>.
        /// For document serialization configuration, <see cref="DocumentSerializationConfigurationFn"/>
        /// </remarks>
        public Func<SerializationConfiguration> EntitySerializationConfigurationFn { get; set; }

        /// <summary>
        /// Used for constructing meta-data about documents used for serialization.
        /// </summary>
        public Func<IDocumentSerializationMetaProvider> DocumentSerializationMetaProviderFn { get; set; }

        /// <summary>
        /// Used e.g. for boostraping components that needs to be able to read and set values
        /// effectively to entities. Used e.g. in <see cref="IEntities.Reflector"/>.
        /// </summary>
        public Func<IEntityReflector> EntityReflectorFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchClient.Serializer"/>.
        /// </summary>
        public Func<ISerializer> SerializerFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IDocuments.Serializer"/>.
        /// </summary>
        public Func<IDocumentSerializer> DocumentSerializerFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IEntities.Serializer"/>.
        /// </summary>
        public Func<IEntitySerializer> EntitySerializerFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchClient.Changes"/>.
        /// </summary>
        public Func<IConnection, IChanges> ChangesFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchClient.Attachments"/>.
        /// </summary>
        public Func<IConnection, IAttachments> AttachmentsFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchClient.Databases"/>.
        /// </summary>
        public Func<IConnection, IDatabases> DatabasesFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchClient.Documents"/>.
        /// </summary>
        public Func<IConnection, IDocuments> DocumentsFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchClient.Entities"/>.
        /// </summary>
        public Func<IConnection, IEntities> EntitiesFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchClient.Views"/>.
        /// </summary>
        public Func<IConnection, IViews> ViewsFn { get; set; }

        public MyCouchClientBootstraper()
        {
            ConfigureChangesFn();
            ConfigureAttachmentsFn();
            ConfigureDatabasesFn();
            ConfigureDocumentsFn();
            ConfigureEntitiesFn();
            ConfigureViewsFn();

            ConfigureSerializationConfigurationFn();
            ConfigureDocumentSerializationConfigurationFn();
            ConfigureEntitySerializationConfigurationFn();
            ConfigureDocumentSerializationMetaProvider();

            ConfigureSerializerFn();
            ConfigureDocumentSerializerFn();
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
            DocumentsFn = cn => new Documents(
                cn,
                DocumentSerializerFn());
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
            SerializationConfigurationFn = () =>
            {
                var contractResolver = new SerializationContractResolver();

                return new SerializationConfiguration(contractResolver);
            };
        }

        protected virtual void ConfigureDocumentSerializationConfigurationFn()
        {
            var serializationConfiguration = new Lazy<SerializationConfiguration>(() =>
            {
                var contractResolver = new SerializationContractResolver();

                return new SerializationConfiguration(contractResolver);
            });

            DocumentSerializationConfigurationFn = () => serializationConfiguration.Value;
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

        protected virtual void ConfigureDocumentSerializationMetaProvider()
        {
            var provider = new Lazy<IDocumentSerializationMetaProvider>(() => new DocumentSerializationMetaProvider());

            DocumentSerializationMetaProviderFn = () => provider.Value;
        }

        protected virtual void ConfigureSerializerFn()
        {
            var serializer = new Lazy<ISerializer>(() => new DefaultSerializer(SerializationConfigurationFn()));
            SerializerFn = () => serializer.Value;
        }

        protected virtual void ConfigureDocumentSerializerFn()
        {
            var serializer = new Lazy<IDocumentSerializer>(() => new DocumentSerializer(DocumentSerializationConfigurationFn(), DocumentSerializationMetaProviderFn()));
            DocumentSerializerFn = () => serializer.Value;
        }

        protected virtual void ConfigureEntitySerializerFn()
        {
            var serializer = new Lazy<IEntitySerializer>(() => new EntitySerializer(EntitySerializationConfigurationFn(), DocumentSerializationMetaProviderFn()));
            EntitySerializerFn = () => serializer.Value;
        }
    }
}