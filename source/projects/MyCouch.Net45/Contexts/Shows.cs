using EnsureThat;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCouch.Contexts
{
    public class Shows : ApiContextBase<IDbClientConnection>, IShows
    {
        protected RawResponseFactory RawResponseFactory { get; set; }
        public Shows(IDbClientConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();
            RawResponseFactory = new RawResponseFactory(serializer);
        }
    }
}
