using FluentAssertions;
using Xunit;

namespace MyCouch.UnitTests
{
    public class UriBuilderTests : UnitTestsOf<MyCouchUriBuilder>
    {
        [Fact]
        public void Can_map_schemes()
        {
            var builder1 = new MyCouchUriBuilder("http://foo.myhost.com:5984");
            var builder2 = new MyCouchUriBuilder("https://foo.myhost.com:5984");

            var uri1 = builder1.Build();
            var uri2 = builder2.Build();

            uri1.Scheme.Should().Be("http");
            uri2.Scheme.Should().Be("https");
        }

        [Fact]
        public void Can_map_authorities()
        {
            var builder1 = new MyCouchUriBuilder("http://foo.myhost.com:5984");
            var builder2 = new MyCouchUriBuilder("http://tada.com:5984");

            var uri1 = builder1.Build();
            var uri2 = builder2.Build();

            uri1.Authority.Should().Be("foo.myhost.com:5984");
            uri2.Authority.Should().Be("tada.com:5984");
        }

        [Fact]
        public void Can_map_ports()
        {
            var builder1 = new MyCouchUriBuilder("http://foo.myhost.com");
            var builder2 = new MyCouchUriBuilder("http://foo.myhost.com:5984");

            var uri1 = builder1.Build();
            var uri2 = builder2.Build();

            uri1.Port.Should().Be(80);
            uri2.Port.Should().Be(5984);
        }

        [Fact]
        public void Can_map_dbname_in_constructor()
        {
            var builder1 = new MyCouchUriBuilder("http://foo.myhost.com/dbname1");
            var builder2 = new MyCouchUriBuilder("http://foo.myhost.com:5984/dbname2");

            var uri1 = builder1.Build();
            var uri2 = builder2.Build();

            uri1.LocalPath.Should().Be("/dbname1");
            uri2.LocalPath.Should().Be("/dbname2");
        }

        [Fact]
        public void When_localpath_with_an_ending_slash_It_will_not_append_a_slash()
        {
            SUT = new MyCouchUriBuilder("http://foo.myhost.com/mydb/");

            var uri = SUT.Build();

            uri.ToString().Should().Be("http://foo.myhost.com/mydb");
        }

        [Fact]
        public void When_setting_dbname_It_will_return_uri_with_dbname()
        {
            SUT = new MyCouchUriBuilder("https://foo.myhost.com:5984");

            var uri = SUT
                .SetDbName("mydb")
                .Build();

            uri.ToString().Should().Be("https://foo.myhost.com:5984/mydb");
        }

        [Fact]
        public void When_setting_basic_credentials_It_will_return_uri_with_credentials()
        {
            SUT = new MyCouchUriBuilder("https://foo.myhost.com:5984/mydb");

            var uri = SUT
                .SetBasicCredentials("theuser", "thepassword")
                .Build();

            uri.ToString().Should().Be("https://theuser:thepassword@foo.myhost.com:5984/mydb");
        }

        [Fact]
        public void When_setting_basic_credentials_with_funny_char_in_credentials_It_will_return_uri_with_proper_credentials()
        {
            SUT = new MyCouchUriBuilder("https://foo.myhost.com:5984/mydb");

            var uri = SUT
                .SetBasicCredentials("the@user", "p@ssword")
                .Build();

            uri.ToString().Should().Be("https://the%40user:p%40ssword@foo.myhost.com:5984/mydb");
        }
    }
}
