using System;
using System.Linq;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch
{
    public class DbConnectionInfo : ConnectionInfo
    {
        public string DbName { get; }

        public DbConnectionInfo(string serverAddress, string dbName) : this(new Uri(serverAddress), dbName) { }
        public DbConnectionInfo(Uri serverAddress, string dbName) : base(UriMagic.Abracadabra(serverAddress.OriginalString, dbName))
        {
            Ensure.String.IsNotNullOrWhiteSpace(dbName, nameof(dbName));

            DbName = dbName.Trim(' ', '/');
        }
    }

    public class ServerConnectionInfo : ConnectionInfo
    {
        public ServerConnectionInfo(string serverAddress) : this(new Uri(serverAddress)) { }
        public ServerConnectionInfo(Uri serverAddress) : base(serverAddress) { }
    }

    public abstract class ConnectionInfo
    {
        public Uri Address { get; }
        public TimeSpan? Timeout { get; set; }
        public BasicAuthString BasicAuth { get; set; }
        public bool AllowAutoRedirect { get; set; } = false;
        public bool ExpectContinue { get; set; } = false;
        public bool UseProxy { get; set; } = true;
        public IWebProxy Proxy { get; set; } = null;

        protected ConnectionInfo(Uri address)
        {
            Ensure.Any.IsNotNull(address, nameof(address));

            Address = RemoveUserInfoFrom(address);

            if (!string.IsNullOrWhiteSpace(address.UserInfo))
            {
                var userInfoParts = ExtractUserInfoPartsFrom(address);
                BasicAuth = new BasicAuthString(userInfoParts[0], userInfoParts[1]);
            }
        }

        private Uri RemoveUserInfoFrom(Uri address)
        {
            return new Uri(address.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.UserInfo, UriFormat.UriEscaped));
        }

        private string[] ExtractUserInfoPartsFrom(Uri address)
        {
            return address.UserInfo
                .Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(Uri.UnescapeDataString)
                .ToArray();
        }
    }
}
