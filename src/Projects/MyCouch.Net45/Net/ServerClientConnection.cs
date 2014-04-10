using System;

namespace MyCouch.Net
{
    public class ServerClientConnection : Connection, IServerClientConnection
    {
        public ServerClientConnection(Uri dbUri) : base(dbUri)
        {
            var dbName = Address.LocalPath.TrimStart('/').TrimEnd('/', '?');
            if (!string.IsNullOrWhiteSpace(dbName))
            {
#if NETFX_CORE
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