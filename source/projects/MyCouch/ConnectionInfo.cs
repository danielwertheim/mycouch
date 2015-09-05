using System;
using System.Linq;
using MyCouch.EnsureThat;
using MyCouch.Net;

namespace MyCouch
{
#if !PCL
    [Serializable]
#endif
    public class DbConnectionInfo : ConnectionInfo
    {
        public string DbName { get; private set; }

        public DbConnectionInfo(Uri serverAddress, string dbName) : base(serverAddress)
        {
            Ensure.That(dbName, "dbName").IsNotNullOrWhiteSpace();

            DbName = dbName;
        }

        public override string GetAddressExceptUserInfo()
        {
            return $"{ServerAddress.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.UserInfo, UriFormat.UriEscaped)}{DbName}";
        }
    }

#if !PCL
    [Serializable]
#endif
    public class ServerConnectionInfo : ConnectionInfo
    {
        public ServerConnectionInfo(Uri serverAddress) : base(serverAddress) { }

        public override string GetAddressExceptUserInfo()
        {
            return ServerAddress.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.UserInfo, UriFormat.UriEscaped).TrimEnd('/');
        }
    }

#if !PCL
    [Serializable]
#endif
    public abstract class ConnectionInfo
    {
        public Uri ServerAddress { get; }
        public TimeSpan? Timeout { get; set; }

        protected ConnectionInfo(Uri serverAddress)
        {
            Ensure.That(serverAddress, "serverAddress").IsNotNull();

            ServerAddress = serverAddress;
        }

        public abstract string GetAddressExceptUserInfo();

        public virtual BasicAuthString GetBasicAuthString()
        {
            if (string.IsNullOrWhiteSpace(ServerAddress.UserInfo))
                return null;

            var parts = GetUserInfoParts();

            return new BasicAuthString(parts[0], parts[1]);
        }

        public virtual string[] GetUserInfoParts()
        {
            return ServerAddress.UserInfo
                .Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(Uri.UnescapeDataString)
                .ToArray();
        }
    }
}