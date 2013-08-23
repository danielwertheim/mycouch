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
        public Func<IEntityReflector> EntityReflectorResolver { get; set; }
        public Func<IConnection, IEntities> EntitiesResolver { get; set; }

        public RichClientBootstraper()
        {
            ConfigureEntitiesResolver();
            ConfigureEntityReflectorResolver();
            ConfigureContractResolver();
            ConfigureSerializer();
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

        private void ConfigureSerializer()
        {
            var serializer = new Lazy<ISerializer>(() => new EntityEnabledSerializer(SerializationConfigurationResolver()));
            SerializerResolver = () => serializer.Value;
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