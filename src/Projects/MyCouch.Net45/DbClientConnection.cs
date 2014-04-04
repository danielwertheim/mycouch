using System;
using EnsureThat;

namespace MyCouch
{
    public class DbClientConnection : Connection, IDbClientConnection
    {
        public string DbName { get; private set; }

        public DbClientConnection(Uri dbUri, string dbName = null) : base(dbUri)
        {
            if(DbName != null)
                Ensure.That(dbName, "dbName").IsNotNullOrWhiteSpace();

            DbName = dbName ?? ExtractDbName();
        }

        private string ExtractDbName()
        {
            var dbName = Address.LocalPath.TrimStart('/').TrimEnd('/', '?');
            if (string.IsNullOrWhiteSpace(dbName))
            {
#if NETFX_CORE
                throw new FormatException(
                    string.Format(ExceptionStrings.CanNotExtractDbNameFromDbUri, Address.OriginalString));
#else
                throw new UriFormatException(
                    string.Format(ExceptionStrings.CanNotExtractDbNameFromDbUri, Address.OriginalString));
#endif
            }

            return dbName;
        }
    }
}