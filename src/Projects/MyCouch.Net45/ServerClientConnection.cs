using System;

namespace MyCouch
{
    public class ServerClientConnection : Connection, IServerClientConnection
    {
        public ServerClientConnection(Uri dbUri) : base(dbUri) { }
    }
}