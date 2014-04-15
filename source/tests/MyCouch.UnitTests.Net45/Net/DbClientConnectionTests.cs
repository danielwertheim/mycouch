using System;
using FluentAssertions;
using MyCouch.Net;
using Xunit;

namespace MyCouch.UnitTests.Net
{
    public class DbClientConnectionTests : UnitTestsOf<DbClientConnection>
    {
        [Fact]
        public void When_created_with_uri_not_ending_with_a_db_name_It_throws_an_exception()
        {
            var uri = new Uri("http://foo:5555");

            Action a = () =>
            {
                SUT = new DbClientConnection(uri);
            };

#if PCL
            a.ShouldThrow<FormatException>().WithMessage(string.Format(ExceptionStrings.CanNotExtractDbNameFromDbUri, uri.OriginalString));
#else
            a.ShouldThrow<UriFormatException>().WithMessage(string.Format(ExceptionStrings.CanNotExtractDbNameFromDbUri, uri.OriginalString));
#endif
        }

        [Fact]
        public void When_created_with_uri_ending_with_db_name_It_can_extract_the_name()
        {
            SUT = new DbClientConnection(new Uri("http://foo:5555/mydb"));

            SUT.DbName.Should().Be("mydb");
        }

        [Fact]
        public void When_created_with_uri_ending_with_db_name_and_slash_It_can_extract_the_name()
        {
            SUT = new DbClientConnection(new Uri("http://foo:5555/mydb/"));

            SUT.DbName.Should().Be("mydb");
        }

        [Fact]
        public void When_created_with_uri_ending_with_db_name_and_question_mark_It_can_extract_the_name()
        {
            SUT = new DbClientConnection(new Uri("http://foo:5555/mydb?"));

            SUT.DbName.Should().Be("mydb");
        }

        [Fact]
        public void When_created_with_uri_ending_with_db_name_slash_and_question_mark_It_can_extract_the_name()
        {
            SUT = new DbClientConnection(new Uri("http://foo:5555/mydb/?"));

            SUT.DbName.Should().Be("mydb");
        }
    }
}