using System;
using MyCouch.Contexts;
using MyCouch.EntitySchemes;
using MyCouch.EntitySchemes.Reflections;
using MyCouch.Serialization;
using MyCouch.Serialization.Meta;

namespace MyCouch
{
    public class MyCouchClientBootstrapper
    {
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
        /// Used e.g. for bootstraping <see cref="IMyCouchClient.DocumentSerializer"/>.
        /// </summary>
        public Func<ISerializer> DocumentSerializerFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchClient.Changes"/>.
        /// </summary>
        public Func<IDbClientConnection, IChanges> ChangesFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchClient.Attachments"/>.
        /// </summary>
        public Func<IDbClientConnection, IAttachments> AttachmentsFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchClient.Database"/>.
        /// </summary>
        public Func<IDbClientConnection, IDatabase> DatabaseFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchServerClient.Databases"/>.
        /// </summary>
        public Func<IServerClientConnection, IDatabases> DatabasesFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchServerClient.Replicator"/>.
        /// </summary>
        public Func<IServerClientConnection, IReplicator> ReplicatorFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchClient.Documents"/>.
        /// </summary>
        public Func<IDbClientConnection, IDocuments> DocumentsFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchClient.Entities"/>.
        /// </summary>
        public Func<IDbClientConnection, IEntities> EntitiesFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchClient.Views"/>.
        /// </summary>
        public Func<IDbClientConnection, IViews> ViewsFn { get; set; }

        public MyCouchClientBootstrapper()
        {
            ConfigureChangesFn();
            ConfigureAttachmentsFn();
            ConfigureDatabaseFn();
            ConfigureDatabasesFn();
            ConfigureReplicatorFn();
            ConfigureDocumentsFn();
            ConfigureEntitiesFn();
            ConfigureViewsFn();

            ConfigureSerializerFn();
            ConfigureDocumentSerializerFn();
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

        protected virtual void ConfigureDatabaseFn()
        {
            DatabaseFn = cn => new Database(cn, SerializerFn());
        }

        protected virtual void ConfigureDatabasesFn()
        {
            DatabasesFn = cn => new Databases(cn, SerializerFn());
        }

        protected virtual void ConfigureReplicatorFn()
        {
            ReplicatorFn = cn => new Replicator(cn, SerializerFn());
        }

        protected virtual void ConfigureDocumentsFn()
        {
            DocumentsFn = cn => new Documents(
                cn,
                SerializerFn());
        }

        protected virtual void ConfigureEntitiesFn()
        {
            EntitiesFn = cn => new Entities(
                cn,
                DocumentSerializerFn(),
                EntityReflectorFn());
        }

        protected virtual void ConfigureViewsFn()
        {
            ViewsFn = cn => new Views(
                cn,
                DocumentSerializerFn());
        }

        protected virtual void ConfigureEntityReflectorFn()
        {
#if !PCL
            var entityReflector = new Lazy<IEntityReflector>(() => new EntityReflector(new IlDynamicPropertyFactory()));
#else
            var entityReflector = new Lazy<IEntityReflector>(() => new EntityReflector(new LambdaDynamicPropertyFactory()));
#endif
            EntityReflectorFn = () => entityReflector.Value;
        }

        protected virtual void ConfigureSerializerFn()
        {
            var serializer = new Lazy<ISerializer>(() =>
            {
                var contractResolver = new SerializationContractResolver();
                var documentMetaProvider = new EmptyDocumentSerializationMetaProvider();
                var configuration = new SerializationConfiguration(contractResolver);

                return new DefaultSerializer(configuration, documentMetaProvider);
            });
            SerializerFn = () => serializer.Value;
        }

        protected virtual void ConfigureDocumentSerializerFn()
        {
            var serializer = new Lazy<ISerializer>(() =>
            {
                var contractResolver = new SerializationContractResolver();
                var documentMetaProvider = new DocumentSerializationMetaProvider();
                var configuration = new SerializationConfiguration(contractResolver);

                return new DefaultSerializer(configuration, documentMetaProvider, EntityReflectorFn());
            });
            DocumentSerializerFn = () => serializer.Value;
        }
    }
}