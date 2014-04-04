using System;
using EnsureThat;

namespace MyCouch.Net
{
    public class DbClientConnectionDbRequestUrlGenerator : IDbRequestUrlGenerator
    {
        private readonly IDbClientConnection _connection;

        public DbClientConnectionDbRequestUrlGenerator(IDbClientConnection connection)
        {
            Ensure.That(connection, "connection").IsNotNull();

            _connection = connection;
        }

        public string Generate(string dbName)
        {
            if (_connection.DbName != dbName)
                throw new InvalidOperationException(ExceptionStrings.DbRequestUrlIsAgainstOtherDb);

            return _connection.Address.AbsoluteUri;
        }
    }
}