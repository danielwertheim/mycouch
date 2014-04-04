using System;

namespace MyCouch.Net
{
    public class ServerClientConnection : Connection, IServerClientConnection
    {
        public ServerClientConnection(Uri dbUri) : base(dbUri) { }
    }
}