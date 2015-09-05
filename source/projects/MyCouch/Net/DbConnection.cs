using System;

namespace MyCouch.Net
{
    public class DbConnection : Connection, IDbConnection
    {
        public string DbName { get; }

        public DbConnection(DbConnectionInfo connectionInfo) : base(connectionInfo)
        {
            DbName = connectionInfo.DbName;
        }
    }
}