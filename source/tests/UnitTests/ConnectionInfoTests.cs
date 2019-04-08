using System;
using FluentAssertions;
using MyCouch;
using Xunit;

namespace UnitTests
{
    public class DbConnectionInfoTests : UnitTestsOf<DbConnectionInfo>
    {
        [Fact]
        public void When_created_with_user_info_It_should_initialize_properly()
        {
            SUT = new DbConnectionInfo("http://s%40:p%40ssword@localhost:5984", "mydb");

            SUT.Address.Should().Be(new Uri("http://localhost:5984/mydb"));
            SUT.DbName.Should().Be("mydb");
            SUT.BasicAuth.Should().NotBeNull();
            SUT.BasicAuth.Value.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void When_created_without_user_info_It_should_initialize_properly()
        {
            SUT = new DbConnectionInfo("http://localhost:5984", "mydb");

            SUT.Address.Should().Be(new Uri("http://localhost:5984/mydb"));
            SUT.DbName.Should().Be("mydb");
            SUT.BasicAuth.Should().BeNull();
        }

        [Theory]
        [InlineData("http://mydomain.com:8081/cloud/", "/mydb/", "http://mydomain.com:8081/cloud/mydb")]
        [InlineData("http://mydomain.com:8081/cloud/", "/mydb", "http://mydomain.com:8081/cloud/mydb")]
        [InlineData("http://mydomain.com:8081/cloud/", "mydb/", "http://mydomain.com:8081/cloud/mydb")]
        [InlineData("http://mydomain.com:8081/cloud/", "mydb", "http://mydomain.com:8081/cloud/mydb")]
        [InlineData("https://mydomain.com:443/cloud/", "mydb", "https://mydomain.com/cloud/mydb")]
        public void When_created_with_segments_It_should_initialize_properly(string baseAddress, string dbName, string expectedResult)
        {
            SUT = new DbConnectionInfo(baseAddress, dbName);

            SUT.Address.OriginalString.Should().Be(expectedResult);
            SUT.DbName.Should().Be("mydb");
            SUT.BasicAuth.Should().BeNull();
        }
    }

    public class ServerConnectionInfoTests : UnitTestsOf<ServerConnectionInfo>
    {
        [Fact]
        public void When_created_with_user_info_It_should_initialize_properly()
        {
            SUT = new ServerConnectionInfo("http://s%40:p%40ssword@localhost:5984");

            SUT.Address.Should().Be(new Uri("http://localhost:5984"));
            SUT.BasicAuth.Should().NotBeNull();
            SUT.BasicAuth.Value.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void When_created_without_user_info_It_should_initialize_properly()
        {
            SUT = new ServerConnectionInfo("http://localhost:5984");

            SUT.Address.Should().Be(new Uri("http://localhost:5984"));
            SUT.BasicAuth.Should().BeNull();
        }
    }
}