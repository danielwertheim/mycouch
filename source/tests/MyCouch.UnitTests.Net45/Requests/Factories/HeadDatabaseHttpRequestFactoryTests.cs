using System;
using FluentAssertions;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Requests.Factories;
using MyCouch.UnitTests.Fakes;
using Xunit;

namespace MyCouch.UnitTests.Requests.Factories
{
    public class HeadDatabaseHttpRequestFactoryTests : UnitTestsOf<HeadDatabaseHttpRequestFactory>
    {
        [Fact]
        public void When_used_with_DbClientConnection_It_generates_correct_url()
        {
            var connection = new DbClientConnectionFake(new Uri("http://foo.com:5984/thedb"), "thedb");
            SUT = new HeadDatabaseHttpRequestFactory(connection);

            var r = SUT.Create(new HeadDatabaseRequest(connection.DbName));

            r.RequestUri.AbsoluteUri.Should().Be("http://foo.com:5984/thedb");
        }

        [Fact]
        public void When_used_with_ServerClientConnection_It_generates_correct_url()
        {
            var connection = new ServerClientConnectionFake(new Uri("http://foo.com:5984"));
            SUT = new HeadDatabaseHttpRequestFactory(connection);

            var r = SUT.Create(new HeadDatabaseRequest("otherdb"));

            r.RequestUri.AbsoluteUri.Should().Be("http://foo.com:5984/otherdb");
        }
    }
}