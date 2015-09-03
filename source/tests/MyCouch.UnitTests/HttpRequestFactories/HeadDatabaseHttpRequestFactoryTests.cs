using FluentAssertions;
using MyCouch.HttpRequestFactories;
using MyCouch.Requests;
using Xunit;

namespace MyCouch.UnitTests.HttpRequestFactories
{
    public class HeadDatabaseHttpRequestFactoryTests : UnitTestsOf<HeadDatabaseHttpRequestFactory>
    {
        [Fact]
        public void When_passing_db_name_It_should_generate_a_relative_url()
        {
            SUT = new HeadDatabaseHttpRequestFactory();

            var r = SUT.Create(new HeadDatabaseRequest("theDb"));

            r.RelativeUrl.Should().Be("/");
        }
    }
}