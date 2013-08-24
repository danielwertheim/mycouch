using System;
using MyCouch.Net;
using MyCouch.ResponseFactories;
using MyCouch.Rich.EntitySchemes;
using MyCouch.Rich.EntitySchemes.Reflections;
using MyCouch.Rich.ResponseFactories;
using MyCouch.Rich.Serialization;
using MyCouch.Serialization;

namespace MyCouch.Rich
{
    public class RichClientBootstraper : LightClientBootsraper
    {
        public Func<EntityReflector> EntityReflectorResolver { get; set; } 
        public Func<IConnection, IEntities> EntitiesResolver { get; set; }

        public RichClientBootstraper()
        {
            ConfigureEntityReflectorResolver();
            ConfigureViewsResolver();
            ConfigureEntitiesResolver();
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

        private void ConfigureViewsResolver()
        {
            var contractResolver = new EntitySerializationContractResolver(EntityReflectorResolver());
            var serializationConfiguration = new SerializationConfiguration(contractResolver);

            var responseMaterializer = new DefaultResponseMaterializer(serializationConfiguration);

            ViewsResolver = cn => new Views(
                cn,
                new JsonViewQueryResponseFactory(responseMaterializer),
                new ViewQueryResponseFactory(responseMaterializer));
        }

        private void ConfigureEntitiesResolver()
        {
#if !NETFX_CORE
            var entityReflector = new EntityReflector(new IlDynamicPropertyFactory());
#else
            var entityReflector = new EntityReflector(new LambdaDynamicPropertyFactory());
#endif
            var contractResolver = new EntitySerializationContractResolver(entityReflector);
            var serializationConfiguration = new SerializationConfiguration(contractResolver)
            {
                WriterFactory = (t, w) => new EntityJsonWriter(t, w)
            };
            var serializer = new DefaultSerializer(serializationConfiguration);
            var responseMaterializer = new DefaultResponseMaterializer(serializationConfiguration);

            EntitiesResolver = cn => new Entities(
                cn,
                new EntityResponseFactory(responseMaterializer, serializer),
                serializer,
                entityReflector);
        }
    }
}