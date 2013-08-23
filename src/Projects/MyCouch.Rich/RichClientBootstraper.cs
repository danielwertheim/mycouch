using System;
using MyCouch.Net;
using MyCouch.Rich.EntitySchemes;
using MyCouch.Rich.EntitySchemes.Reflections;
using MyCouch.Rich.ResponseFactories;
using MyCouch.Rich.Serialization;
using MyCouch.Serialization;

namespace MyCouch.Rich
{
    public class RichClientBootstraper : LightClientBootsraper
    {
        public Func<IEntityReflector> EntityReflectorResolver { get; set; }
        public Func<IConnection, IEntities> EntitiesResolver { get; set; }

        public RichClientBootstraper()
        {
            ConfigureEntitiesResolver();
            ConfigureEntityReflectorResolver();
            ConfigureSerializationConfigurationResolver();
        }

        private void ConfigureEntityReflectorResolver()
        {
#if !NETFX_CORE
            var entityReflector = new EntityReflector(new IlDynamicPropertyFactory());
#else
            var entityReflector = new EntityReflector(new LambdaDynamicPropertyFactory());
#endif
            EntityReflectorResolver = () => entityReflector;
        }

        private void ConfigureSerializationConfigurationResolver()
        {
            var serializationConfiguration = new SerializationConfiguration(new RichSerializationContractResolver(EntityReflectorResolver()));
            SerializationConfigurationResolver = () => serializationConfiguration;
        }

        private void ConfigureEntitiesResolver()
        {

            EntitiesResolver = cn => new Entities(cn,
                new EntityResponseFactory(ResponseMaterializerResolver(), SerializerResolver()),
                SerializerResolver(),
                EntityReflectorResolver());
        }
    }
}