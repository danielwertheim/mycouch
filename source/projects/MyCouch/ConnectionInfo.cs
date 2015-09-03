using System;
using System.Linq;
using MyCouch.EnsureThat;
using MyCouch.Net;

namespace MyCouch
{
#if !PCL
    [Serializable]
#endif
    public class ConnectionInfo
    {
        public Uri Address { get; private set; }
        public string DbName { get; private set; }
        public TimeSpan? Timeout { get; set; }

        public ConnectionInfo(Uri address, string dbName = null)
        {
            Ensure.That(address, "address").IsNotNull();

            Address = address;
            DbName = dbName ?? ExtractDbName(address);
        }

        private static string ExtractDbName(Uri value)
        {
            return value.LocalPath.TrimStart('/').TrimEnd('/', '?').Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        }

        public virtual string GetAbsoluteAddressExceptUserInfo()
        {
            return Address.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.UserInfo, UriFormat.UriEscaped);
        }

        public virtual BasicAuthString GetBasicAuthString()
        {
            if (string.IsNullOrWhiteSpace(Address.UserInfo))
                return null;

            var parts = GetUserInfoParts();

            return new BasicAuthString(parts[0], parts[1]);
        }

        public virtual string[] GetUserInfoParts()
        {
            return Address.UserInfo
                .Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => Uri.UnescapeDataString(p))
                .ToArray();
        }
    }
}