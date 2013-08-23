using System;
using MyCouch.Net;
using MyCouch.Rich.EntitySchemes;
using MyCouch.Rich.EntitySchemes.Reflections;
using MyCouch.Rich.Responses;
using MyCouch.Rich.Serialization;

namespace MyCouch.Rich
{
    public class RichClient : Client, IRichClient
    {
        public IEntities Entities { get; protected set; }

        public RichClient(string url) : this(new Uri(url)) { }

        public RichClient(Uri uri) : this(new BasicHttpClientConnection(uri)) { }

        public RichClient(IConnection connection) : base(connection)
        {
#if !NETFX_CORE
            var entityReflector = new EntityReflector(new IlDynamicPropertyFactory());
#else
            var entityReflector = new EntityReflector(new LambdaDynamicPropertyFactory());
#endif
            Serializer = new RichSerializer(new RichSerializationContractResolver(entityReflector));
            Entities = new Entities(Connection, new EntityResponseFactory(ResponseMaterializer, Serializer), Serializer, entityReflector);
        }
    }
}