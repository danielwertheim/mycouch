using System;
using MyCouch.Net;
using MyCouch.Rich.Schemes;
using MyCouch.Rich.Schemes.Reflections;
using MyCouch.Rich.Serialization;
using MyCouch.Serialization;

namespace MyCouch.Rich
{
    public class RichClient : Client, IRichClient
    {
        public new IRichSerializer Serializer { get; private set; }
        public new IRichResponseFactory ResponseFactory { get; private set; }
        protected IEntityReflector EntityReflector { get; set; }
        public IEntities Entities { get; protected set; }

        public RichClient(string url) : this(new Uri(url)) { }

        public RichClient(Uri uri) : this(new BasicHttpClientConnection(uri)) { }

        public RichClient(IConnection connection) : base(connection)
        {
#if !NETFX_CORE
            EntityReflector = new EntityReflector(new IlDynamicPropertyFactory());
#else
            EntityReflector = new EntityReflector(new LambdaDynamicPropertyFactory());
#endif
            Serializer = new RichSerializer(new RichSerializationContractResolver(() => EntityReflector));
            ResponseFactory = new RichResponseFactory(new ResponseMaterializer(), Serializer);
            Entities = new Entities(Connection, ResponseFactory, Serializer, EntityReflector);
        }
    }
}