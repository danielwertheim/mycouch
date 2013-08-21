using System;
using MyCouch.Net;
using MyCouch.Rich.Serialization;
using MyCouch.Schemes;
using MyCouch.Schemes.Reflections;
using MyCouch.Serialization;

namespace MyCouch.Rich
{
    public class RichClient : Client, IRichClient
    {
        public IRichSerializer Serializer { get; set; }
        public IRichResponseFactory ResponseFactory { get; set; }
        public IEntityReflector EntityReflector { get; set; }
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
            Serializer = new RichSerializer(() => EntityReflector);
            ResponseFactory = new RichResponseFactory(new ResponseMaterializer(), Serializer);
            Entities = new Entities(this);
        }
    }
}