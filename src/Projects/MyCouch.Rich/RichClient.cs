using System;
using MyCouch.Net;
using MyCouch.Responses;
using MyCouch.Rich.Responses;
using MyCouch.Rich.Schemes;
using MyCouch.Rich.Schemes.Reflections;
using MyCouch.Rich.Serialization;

namespace MyCouch.Rich
{
    public class RichClient : Client, IRichClient
    {
        public new IRichSerializer Serializer { get; private set; }
        public new IRichResponseFactory ResponseFactory { get; private set; }
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
            ResponseFactory = new RichResponseFactory(new DefaultResponseMaterializer(), Serializer);
            Entities = new Entities(Connection, ResponseFactory, Serializer, entityReflector);
        }
    }
}