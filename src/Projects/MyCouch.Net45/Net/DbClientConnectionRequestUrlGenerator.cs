using System;
using EnsureThat;

namespace MyCouch.Net
{
    public class DbClientConnectionRequestUrlGenerator : IRequestUrlGenerator
    {
        private readonly IDbClientConnection _connection;

        public DbClientConnectionRequestUrlGenerator(IDbClientConnection connection)
        {
            Ensure.That(connection, "connection").IsNotNull();

            _connection = connection;
        }

        public string Generate(string resourceName)
        {
            if (_connection.DbName != resourceName)
                throw new InvalidOperationException(ExceptionStrings.DbRequestUrlIsAgainstOtherDb);

            return _connection.Address.AbsoluteUri;
        }
    }
}