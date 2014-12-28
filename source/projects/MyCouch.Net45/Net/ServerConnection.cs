using System;

namespace MyCouch.Net
{
    public class ServerConnection : Connection, IServerConnection
    {
        public ServerConnection(ConnectionInfo connectionInfo) : base(connectionInfo)
        {
            if (!string.IsNullOrWhiteSpace(connectionInfo.DbName))
            {
#if PCL
                throw new FormatException(
                    string.Format(ExceptionStrings.ServerClientSeemsToConnectToDb, Address.OriginalString));
#else
                throw new UriFormatException(
                    string.Format(ExceptionStrings.ServerClientSeemsToConnectToDb, Address.OriginalString));
#endif
            }
        }
    }
}