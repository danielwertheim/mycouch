using System;
using FluentAssertions;
using Xunit;

namespace MyCouch.UnitTests
{
    public class DbClientTests : UnitTestsOf<MyCouchClient>
    {
        [Fact]
        public void When_created_with_uri_not_ending_with_a_db_name_It_throws_an_exception()
        {
            const string uri = "http://foo:5555";

            Action a = () =>
            {
                var client = new MyCouchClient(uri);
            };

#if NETFX_CORE
            a.ShouldThrow<FormatException>().And.Message.Should().Be(string.Format(ExceptionStrings.CanNotExtractDbNameFromDbUri, uri));
#else
            a.ShouldThrow<UriFormatException>().And.Message.Should().Be(string.Format(ExceptionStrings.CanNotExtractDbNameFromDbUri, uri));
#endif
        }

        [Fact]
        public void When_created_with_uri_ending_with_db_name_It_can_extract_the_name()
        {
            var client = new MyCouchClient("http://foo:5555/mydb");

            client.DbName.Should().Be("mydb");
        }

        [Fact]
        public void When_created_with_uri_ending_with_db_name_and_slash_It_can_extract_the_name()
        {
            var client = new MyCouchClient("http://foo:5555/mydb/");

            client.DbName.Should().Be("mydb");
        }

        [Fact]
        public void When_created_with_uri_ending_with_db_name_and_question_mark_It_can_extract_the_name()
        {
            var client = new MyCouchClient("http://foo:5555/mydb?");

            client.DbName.Should().Be("mydb");
        }

        [Fact]
        public void When_created_with_uri_ending_with_db_name_slash_and_question_mark_It_can_extract_the_name()
        {
            var client = new MyCouchClient("http://foo:5555/mydb/?");

            client.DbName.Should().Be("mydb");
        }
    }
}