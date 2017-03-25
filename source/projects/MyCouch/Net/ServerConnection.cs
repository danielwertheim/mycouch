using System;

namespace MyCouch.Net
{
    public class ServerConnection : Connection, IServerConnection
    {
        public ServerConnection(ServerConnectionInfo connectionInfo) : base(connectionInfo) { }
    }
}