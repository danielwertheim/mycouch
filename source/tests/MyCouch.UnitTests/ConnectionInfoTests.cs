using System;
using FluentAssertions;
using MyCouch.Extensions;
using Xunit;

namespace MyCouch.UnitTests
{
    public class DbConnectionInfoTests : ConnectionInfoTests
    {
        [Fact]
        public void Getting_address_except_userInfo_When_user_info_exists_It_should_remove_user_info_and_append_db()
        {
            var r = new DbConnectionInfo(new Uri("http://s%40:p%40ssword@localhost:5984"), "mydb").GetAddressExceptUserInfo();

            r.Should().Be("http://localhost:5984/mydb");
        }

        [Fact]
        public void Getting_address_except_userInfo_When_no_user_info_exists_It_should_return_uri_with_db()
        {
            var r = new DbConnectionInfo(new Uri("http://localhost:5984"), "mydb").GetAddressExceptUserInfo();

            r.Should().Be("http://localhost:5984/mydb");
        }
    }

    public class ServerConnectionInfoTests : ConnectionInfoTests
    {
        [Fact]
        public void Getting_address_except_userInfo_When_user_info_exists_It_should_remove_user_info()
        {
            var r = new ServerConnectionInfo(new Uri("http://s%40:p%40ssword@localhost:5984")).GetAddressExceptUserInfo();

            r.Should().Be("http://localhost:5984");
        }

        [Fact]
        public void Getting_address_except_userInfo_When_no_user_info_exists_It_should_return_uri()
        {
            var r = new ServerConnectionInfo(new Uri("http://localhost:5984")).GetAddressExceptUserInfo();

            r.Should().Be("http://localhost:5984");
        }
    }

    public abstract class ConnectionInfoTests : UnitTests
    {
        [Fact]
        public void Getting_user_info_parts_When_encoded_user_and_password_are_provided_It_extracts_decoded_user_and_password()
        {
            var r = new DbConnectionInfo(new Uri("http://s%40:p%40ssword@localhost:5984"), "mydb").GetUserInfoParts();

            r.Should().BeEquivalentTo("s@", "p@ssword");
        }

        [Fact]
        public void Getting_user_info_parts_When_non_encoded_user_and_password_are_provided_It_extracts_user_and_password()
        {
            var r = new DbConnectionInfo(new Uri("http://tstUser:tstPwd@localhost:5984"), "mydb").GetUserInfoParts();

            r.Should().BeEquivalentTo("tstUser", "tstPwd");
        }

        [Fact]
        public void Getting_user_info_parts_When_nothing_is_provided_It_returns_empty_array()
        {
            var r = new DbConnectionInfo(new Uri("http://localhost:5984"), "mydb").GetUserInfoParts();

            r.Should().NotBeNull();
            r.Should().BeEmpty();
        }

        [Fact]
        public void Getting_basic_auth_string_When_encoded_user_and_password_are_provided_It_returns_a_basic_auth_string()
        {
            var r = new DbConnectionInfo(new Uri("http://s%40:p%40ssword@localhost:5984"), "mydb").GetBasicAuthString();

            r.Value.Should().Be("s@:p@ssword".AsBase64Encoded());
        }

        [Fact]
        public void Getting_basic_auth_string_When_non_encoded_user_and_password_are_provided_It_returns_a_basic_auth_string()
        {
            var r = new DbConnectionInfo(new Uri("http://foo:bar@localhost:5984"), "mydb").GetBasicAuthString();

            r.Value.Should().Be("foo:bar".AsBase64Encoded());
        }

        [Fact]
        public void Getting_basic_auth_string_When_nothing_is_provided_It_returns_null()
        {
            var r = new DbConnectionInfo(new Uri("http://localhost:5984"), "mydb").GetBasicAuthString();

            r.Should().BeNull();
        }
    }
}