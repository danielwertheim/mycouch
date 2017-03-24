using System;
using MyCouch.Contexts;
using MyCouch.EntitySchemes;
using MyCouch.EntitySchemes.Reflections;
using MyCouch.Net;
using MyCouch.Serialization;
using MyCouch.Serialization.Meta;

namespace MyCouch
{
    internal static class MyCouchClientBootstrappers
    {
        internal static MyCouchClientBootstrapper Default { get; private set; }

        static MyCouchClientBootstrappers()
        {
            Default = new MyCouchClientBootstrapper();
        }
    }

    public class MyCouchClientBootstrapper
    {
        /// <summary>
        /// Used for creating a <see cref="IDbConnection"/>. Override to inject your custom connection.
        /// </summary>
        public Func<DbConnectionInfo, IDbConnection> DbConnectionFn { get; set; }

        /// <summary>
        /// Used for creating a <see cref="IServerConnection"/>. Override to inject your custom connection.
        /// </summary>
        public Func<ServerConnectionInfo, IServerConnection> ServerConnectionFn { get; set; }
 
        /// <summary>
        /// Used for configuring serializers returned via <see cref="SerializerFn"/>
        /// and <see cref="DocumentSerializerFn"/>.
        /// </summary>
        public Func<SerializationConfiguration> SerializationConfigurationFn { get; set; }

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
        public Func<IDbConnection, IChanges> ChangesFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchClient.Attachments"/>.
        /// </summary>
        public Func<IDbConnection, IAttachments> AttachmentsFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchClient.Database"/>.
        /// </summary>
        public Func<IDbConnection, IDatabase> DatabaseFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchServerClient.Databases"/>.
        /// </summary>
        public Func<IServerConnection, IDatabases> DatabasesFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchServerClient.Replicator"/>.
        /// </summary>
        public Func<IServerConnection, IReplicator> ReplicatorFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchClient.Documents"/>.
        /// </summary>
        public Func<IDbConnection, IDocuments> DocumentsFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchClient.Entities"/>.
        /// </summary>
        public Func<IDbConnection, IEntities> EntitiesFn { get; set; }

        /// <summary>
        /// Used e.g. for bootrstraping <see cref="IMyCouchClient.Queries"/>.
        /// </summary>
        public Func<IDbConnection, IQueries> QueriesFn { get; set; }

        /// <summary>
        /// Used e.g. for bootrstraping <see cref="IMyCouchClient.Searches"/>.
        /// </summary>
        public Func<IDbConnection, ISearches> SearchesFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchClient.Views"/>.
        /// </summary>
        public Func<IDbConnection, IViews> ViewsFn { get; set; }

        public MyCouchClientBootstrapper()
        {
            ConfigureDbConnectionFn();
            ConfigureServerConnectionFn();

            ConfigureEntityReflectorFn();
            ConfigureSerializationConfiguration();
            ConfigureSerializerFn();
            ConfigureDocumentSerializerFn();

            ConfigureChangesFn();
            ConfigureAttachmentsFn();
            ConfigureDatabaseFn();
            ConfigureDatabasesFn();
            ConfigureReplicatorFn();
            ConfigureDocumentsFn();
            ConfigureEntitiesFn();
            ConfigureQueriesFn();
            ConfigureSearchesFn();
            ConfigureViewsFn();
        }

        protected virtual void ConfigureDbConnectionFn()
        {
            DbConnectionFn = cnInfo => new DbConnection(cnInfo);
        }

        protected virtual void ConfigureServerConnectionFn()
        {
            ServerConnectionFn = cnInfo => new ServerConnection(cnInfo);
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

        protected virtual void ConfigureQueriesFn()
        {
            QueriesFn = cn => new Queries(
                cn,
                DocumentSerializerFn(),
                SerializerFn());
        }

        protected virtual void ConfigureSearchesFn()
        {
            SearchesFn = cn => new Searches(
                cn,
                DocumentSerializerFn(),
                SerializerFn());
        }

        protected virtual void ConfigureViewsFn()
        {
            ViewsFn = cn => new Views(
                cn,
                DocumentSerializerFn());
        }

        protected virtual void ConfigureEntityReflectorFn()
        {
//#if NETSTANDARD1_1 || vNext || PCL
//            var entityReflector = new Lazy<IEntityReflector>(() => new EntityReflector(new LambdaDynamicPropertyFactory()));
//#else
            var entityReflector = new Lazy<IEntityReflector>(() => new EntityReflector(new IlDynamicPropertyFactory()));
//#endif
            EntityReflectorFn = () => entityReflector.Value;
        }

        protected virtual void ConfigureSerializerFn()
        {
            var serializer = new Lazy<ISerializer>(() =>
            {
                var documentMetaProvider = new EmptyDocumentSerializationMetaProvider();

                return new DefaultSerializer(SerializationConfigurationFn(), documentMetaProvider);
            });
            SerializerFn = () => serializer.Value;
        }

        protected virtual void ConfigureDocumentSerializerFn()
        {
            var serializer = new Lazy<ISerializer>(() =>
            {
                var documentMetaProvider = new DocumentSerializationMetaProvider();

                return new DefaultSerializer(SerializationConfigurationFn(), documentMetaProvider, EntityReflectorFn());
            });
            DocumentSerializerFn = () => serializer.Value;
        }

        protected virtual void ConfigureSerializationConfiguration()
        {
            var config = new Lazy<SerializationConfiguration>(() =>
            {
                var contractResolver = new SerializationContractResolver();
                return new SerializationConfiguration(contractResolver);
            });
            SerializationConfigurationFn = () => config.Value;
        }
    }
}