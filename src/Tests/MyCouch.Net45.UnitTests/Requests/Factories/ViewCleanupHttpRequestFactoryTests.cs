using System;
using FluentAssertions;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Requests.Factories;
using Xunit;

namespace MyCouch.UnitTests.Requests.Factories
{
    public class ViewCleanupHttpRequestFactoryTests : UnitTestsOf<ViewCleanupHttpRequestFactory>
    {
        private readonly Uri _serverUriFake = new Uri("http://foo.com:5984");
        private readonly Uri _dbUriFake = new Uri("http://foo.com:5984/thedb");

        [Fact]
        public void When_db_client_connection_It_generates_correct_url()
        {
            var connection = new DbClientConnection(_dbUriFake);
            SUT = new ViewCleanupHttpRequestFactory(connection, new DbClientConnectionRequestUrlGenerator(connection));

            var r = SUT.Create(new ViewCleanupRequest(connection.DbName));

            r.RequestUri.AbsoluteUri.Should().Be("http://foo.com:5984/thedb/_view_cleanup");
        }

        [Fact]
        public void When_server_client_connection_It_generates_correct_url()
        {
            var connection = new ServerClientConnection(_serverUriFake);
            SUT = new ViewCleanupHttpRequestFactory(connection, new AppendingRequestUrlGenerator(connection.Address));

            var r = SUT.Create(new ViewCleanupRequest("otherdb"));

            r.RequestUri.AbsoluteUri.Should().Be("http://foo.com:5984/otherdb/_view_cleanup");
        }
    }
}