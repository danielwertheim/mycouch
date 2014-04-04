using System;
using FluentAssertions;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Requests.Factories;
using Xunit;

namespace MyCouch.UnitTests.Requests.Factories
{
    public class PutDatabaseHttpRequestFactoryTests : UnitTestsOf<PutDatabaseHttpRequestFactory>
    {
        private readonly Uri _serverUriFake = new Uri("http://foo.com:5984");
        private readonly Uri _dbUriFake = new Uri("http://foo.com:5984/thedb");

        [Fact]
        public void When_db_client_connection_It_generates_correct_url()
        {
            var connection = new DbClientConnection(_dbUriFake);
            SUT = new PutDatabaseHttpRequestFactory(connection, new DbClientConnectionDbRequestUrlGenerator(connection));

            var r = SUT.Create(new PutDatabaseRequest(connection.DbName));

            r.RequestUri.AbsoluteUri.Should().Be("http://foo.com:5984/thedb");
        }

        [Fact]
        public void When_server_client_connection_It_generates_correct_url()
        {
            var connection = new ServerClientConnection(_serverUriFake);
            SUT = new PutDatabaseHttpRequestFactory(connection, new AppendingDbRequestUrlGenerator(connection.Address));

            var r = SUT.Create(new PutDatabaseRequest("otherdb"));

            r.RequestUri.AbsoluteUri.Should().Be("http://foo.com:5984/otherdb");
        }
    }
}