using System;
using FluentAssertions;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Requests.Factories;
using Xunit;

namespace MyCouch.UnitTests.Requests.Factories
{
    public class GetDatabaseHttpRequestFactoryTests : UnitTestsOf<GetDatabaseHttpRequestFactory>
    {
        private readonly Uri _serverUriFake = new Uri("http://foo.com:5984");
        private readonly Uri _dbUriFake = new Uri("http://foo.com:5984/thedb");

        [Fact]
        public void When_db_client_connection_It_generates_correct_url()
        {
            var connection = new DbClientConnection(_dbUriFake);
            SUT = new GetDatabaseHttpRequestFactory(connection, new DbClientConnectionDbRequestUrlGenerator(connection));

            var r = SUT.Create(new GetDatabaseRequest(connection.DbName));

            r.RequestUri.AbsoluteUri.Should().Be("http://foo.com:5984/thedb");
        }

        [Fact]
        public void When_server_client_connection_It_generates_correct_url()
        {
            var connection = new ServerClientConnection(_serverUriFake);
            SUT = new GetDatabaseHttpRequestFactory(connection, new AppendingDbRequestUrlGenerator(connection.Address));

            var r = SUT.Create(new GetDatabaseRequest("otherdb"));

            r.RequestUri.AbsoluteUri.Should().Be("http://foo.com:5984/otherdb");
        }
    }
}