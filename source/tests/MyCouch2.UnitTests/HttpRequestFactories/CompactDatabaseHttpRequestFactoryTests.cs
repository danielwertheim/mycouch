using FluentAssertions;
using MyCouch.HttpRequestFactories;
using MyCouch.Requests;
using Xunit;

namespace MyCouch.UnitTests.HttpRequestFactories
{
    public class CompactDatabaseHttpRequestFactoryTests : UnitTestsOf<CompactDatabaseHttpRequestFactory>
    {
        [Fact]
        public void When_passing_db_name_It_should_generate_a_relative_url()
        {
            SUT = new CompactDatabaseHttpRequestFactory();

            var r = SUT.Create(new CompactDatabaseRequest("theDb"));

            r.RelativeUrl.Should().Be("/_compact");
        }
    }
}