using FluentAssertions;
using MyCouch.HttpRequestFactories;
using MyCouch.Requests;
using Xunit;

namespace UnitTests.HttpRequestFactories
{
    public class ViewCleanupHttpRequestFactoryTests : UnitTestsOf<ViewCleanupHttpRequestFactory>
    {
        [Fact]
        public void When_passing_db_name_It_should_generate_a_relative_url()
        {
            SUT = new ViewCleanupHttpRequestFactory();

            var r = SUT.Create(new ViewCleanupRequest("theDb"));

            r.RelativeUrl.Should().Be("/_view_cleanup");
        }
    }
}