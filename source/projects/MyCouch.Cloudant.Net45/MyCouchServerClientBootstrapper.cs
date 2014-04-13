using System;
using MyCouch.Contexts;
using MyCouch.Serialization;

namespace MyCouch
{
    public class MyCouchServerClientBootstrapper
    {
        /// <summary>
        /// Used e.g. for bootstraping components relying on plain serialization, <see cref="ISerializer"/>
        /// used in <see cref="IMyCouchServerClient.Serializer"/>.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public Func<SerializationConfiguration> SerializationConfigurationFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchServerClient.Serializer"/>.
        /// </summary>
        public Func<ISerializer> SerializerFn { get; set; }

        /// <summary>
        /// Used e.g. for bootstraping <see cref="IMyCouchServerClient.Databases"/>.
        /// </summary>
        public Func<IServerClientConnection, IDatabases> DatabasesFn { get; set; }

        public MyCouchServerClientBootstrapper()
        {
            ConfigureDatabasesFn();

            ConfigureSerializationConfigurationFn();

            ConfigureSerializerFn();
        }

        protected virtual void ConfigureDatabasesFn()
        {
            DatabasesFn = cn => new Databases(cn, SerializerFn());
        }

        protected virtual void ConfigureSerializationConfigurationFn()
        {
            SerializationConfigurationFn = () =>
            {
                var contractResolver = new SerializationContractResolver();

                return new SerializationConfiguration(contractResolver);
            };
        }

        protected virtual void ConfigureSerializerFn()
        {
            var serializer = new Lazy<ISerializer>(() => new DefaultSerializer(SerializationConfigurationFn()));
            SerializerFn = () => serializer.Value;
        }
    }
}