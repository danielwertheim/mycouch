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
        public Func<IConnection, IEntities> EntitiesResolver { get; set; }

        public RichClientBootstraper()
        {
            ConfigureEntitiesResolver();
        }

        private void ConfigureEntitiesResolver()
        {
#if !NETFX_CORE
            var entityReflector = new EntityReflector(new IlDynamicPropertyFactory());
#else
            var entityReflector = new EntityReflector(new LambdaDynamicPropertyFactory());
#endif
            var contractResolver = new RichSerializationContractResolver(entityReflector);
            var serializationConfiguration = new SerializationConfiguration(contractResolver)
            {
                WriterFactory = (t, w) => new SerializationJsonWriter(t, w)
            };
            var serializer = new DefaultSerializer(serializationConfiguration);

            EntitiesResolver = cn => new Entities(
                cn,
                new EntityResponseFactory(ResponseMaterializerResolver(), serializer),
                serializer,
                entityReflector);
        }
    }
}