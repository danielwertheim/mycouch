using System;
using FluentAssertions;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Requests.Factories;
using MyCouch.UnitTests.Fakes;
using Xunit;

namespace MyCouch.UnitTests.Requests.Factories
{
    public class DeleteDatabaseHttpRequestFactoryTests : UnitTestsOf<DeleteDatabaseHttpRequestFactory>
    {
        [Fact]
        public void When_used_with_DbClientConnection_It_generates_correct_url()
        {
            var connection = new DbClientConnectionFake(new Uri("http://foo.com:5984/thedb"), "thedb");
            SUT = new DeleteDatabaseHttpRequestFactory(connection);

            var r = SUT.Create(new DeleteDatabaseRequest(connection.DbName));

            r.RequestUri.AbsoluteUri.Should().Be("http://foo.com:5984/thedb");
        }

        [Fact]
        public void When_used_with_ServerClientConnection_It_generates_correct_url()
        {
            var connection = new ServerClientConnectionFake(new Uri("http://foo.com:5984"));
            SUT = new DeleteDatabaseHttpRequestFactory(connection);

            var r = SUT.Create(new DeleteDatabaseRequest("otherdb"));

            r.RequestUri.AbsoluteUri.Should().Be("http://foo.com:5984/otherdb");
        }
    }
}