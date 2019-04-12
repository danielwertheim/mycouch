using System;
using FluentAssertions;
using MyCouch;
using MyCouch.Net;
using Xunit;

namespace UnitTests.Net
{
    public class DbConnectionTests : UnitTestsOf<DbConnection>
    {
        [Fact]
        public void When_injected_with_connection_info_with_user_cred_It_extracts_values_properly()
        {
            var cnInfo = new DbConnectionInfo(new Uri("http://fakeuser:fakepwd@foo:5555"), "mydb");

            SUT = new DbConnection(cnInfo);

            SUT.Address.Should().Be(new Uri("http://foo:5555/mydb"));
            SUT.DbName.Should().Be("mydb");
        }

        [Fact]
        public void When_injected_with_connection_info_without_user_cred_It_extracts_values_properly()
        {
            var cnInfo = new DbConnectionInfo(new Uri("http://foo:5555"), "mydb");

            SUT = new DbConnection(cnInfo);

            SUT.Address.Should().Be(new Uri("http://foo:5555/mydb"));
            SUT.DbName.Should().Be("mydb");
        }

        [Fact]
        public void When_injected_with_connection_info_with_https_It_extracts_values_properly()
        {
            var cnInfo = new DbConnectionInfo(new Uri("https://foo:5555"), "mydb");

            SUT = new DbConnection(cnInfo);

            SUT.Address.Should().Be(new Uri("https://foo:5555/mydb"));
            SUT.DbName.Should().Be("mydb");
        }

        [Fact]
        public void When_injected_with_connection_info_with_timeout_It_extracts_value_properly()
        {
            var expectedTimeout = TimeSpan.FromSeconds(13);
            var cnInfo = new DbConnectionInfo(new Uri("https://foo:5555"), "mydb")
            {
                Timeout = expectedTimeout
            };

            SUT = new DbConnection(cnInfo);

            SUT.Timeout.Should().Be(expectedTimeout);
        }
    }

    public class ServerConnectionTests : UnitTestsOf<ServerConnection>
    {
        [Fact]
        public void When_injected_with_connection_info_with_user_cred_It_extracts_values_properly()
        {
            var cnInfo = new ServerConnectionInfo(new Uri("http://fakeuser:fakepwd@foo:5555"));

            SUT = new ServerConnection(cnInfo);

            SUT.Address.Should().Be(new Uri("http://foo:5555"));
        }

        [Fact]
        public void When_injected_with_connection_info_without_user_cred_It_extracts_values_properly()
        {
            var cnInfo = new ServerConnectionInfo(new Uri("http://foo:5555"));

            SUT = new ServerConnection(cnInfo);

            SUT.Address.Should().Be(new Uri("http://foo:5555"));
        }

        [Fact]
        public void When_injected_with_connection_info_with_https_It_extracts_values_properly()
        {
            var cnInfo = new ServerConnectionInfo(new Uri("https://foo:5555"));

            SUT = new ServerConnection(cnInfo);

            SUT.Address.Should().Be(new Uri("https://foo:5555"));
        }

        [Fact]
        public void When_injected_with_connection_info_with_timeout_It_extracts_value_properly()
        {
            var expectedTimeout = TimeSpan.FromSeconds(13);
            var cnInfo = new ServerConnectionInfo(new Uri("https://foo:5555"))
            {
                Timeout = expectedTimeout
            };

            SUT = new ServerConnection(cnInfo);

            SUT.Timeout.Should().Be(expectedTimeout);
        }
    }
}