using System;
using MyCouch.Net;
using MyCouch.Rich.EntitySchemes;
using MyCouch.Rich.EntitySchemes.Reflections;
using MyCouch.Rich.ResponseFactories;
using MyCouch.Rich.Serialization;
using MyCouch.Serialization;
using Newtonsoft.Json.Serialization;

namespace MyCouch.Rich
{
    public class RichClientBootstraper : LightClientBootsraper
    {
        public Func<EntityReflector> EntityReflectorResolver { get; set; }
        public Func<IConnection, IEntities> EntitiesResolver { get; set; }

        public RichClientBootstraper()
        {
            ConfigureEntitiesResolver();
            ConfigureEntityReflectorResolver();
            ConfigureContractResolver();
            ConfigureSerializationConfigurationResolver();
        }

        private void ConfigureEntityReflectorResolver()
        {
#if !NETFX_CORE
            var entityReflector = new Lazy<EntityReflector>(() => new EntityReflector(new IlDynamicPropertyFactory()));
#else
            var entityReflector = new Lazy<EntityReflector>(() => new EntityReflector(new LambdaDynamicPropertyFactory()));
#endif
            EntityReflectorResolver = () => entityReflector.Value;
        }

        private void ConfigureContractResolver()
        {
            var contractResolver = new Lazy<IContractResolver>(() => new RichSerializationContractResolver(EntityReflectorResolver()));
            ContractResolver = () => contractResolver.Value;
        }

        private void ConfigureSerializationConfigurationResolver()
        {
            var serializationConfiguration = new Lazy<SerializationConfiguration>(() => new SerializationConfiguration(ContractResolver())
            {
                WriterFactory = (t, w) => new SerializationJsonWriter(t, w)
            });
            SerializationConfigurationResolver = () => serializationConfiguration.Value;
        }

        private void ConfigureEntitiesResolver()
        {
            EntitiesResolver = cn => new Entities(
                cn,
                new EntityResponseFactory(ResponseMaterializerResolver(), SerializerResolver()),
                SerializerResolver(),
                EntityReflectorResolver());
        }
    }
}