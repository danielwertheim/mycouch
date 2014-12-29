using System;

namespace MyCouch.Net
{
    public class DbConnection : Connection, IDbConnection
    {
        public string DbName { get; private set; }

        public DbConnection(ConnectionInfo connectionInfo) : base(connectionInfo)
        {
            if (string.IsNullOrWhiteSpace(connectionInfo.DbName))
            {
#if PCL
                throw new FormatException(
                    string.Format(ExceptionStrings.CanNotExtractDbNameFromDbUri, Address.OriginalString));
#else
                throw new UriFormatException(
                    string.Format(ExceptionStrings.CanNotExtractDbNameFromDbUri, Address.OriginalString));
#endif
            }

            DbName = connectionInfo.DbName;
        }
    }
}