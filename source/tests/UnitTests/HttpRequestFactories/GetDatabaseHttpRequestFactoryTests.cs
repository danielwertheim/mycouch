using FluentAssertions;
using MyCouch.HttpRequestFactories;
using MyCouch.Requests;
using Xunit;

namespace UnitTests.HttpRequestFactories
{
    public class GetDatabaseHttpRequestFactoryTests : UnitTestsOf<GetDatabaseHttpRequestFactory>
    {
        [Fact]
        public void When_passing_db_name_It_should_generate_a_relative_url()
        {
            SUT = new GetDatabaseHttpRequestFactory();

            var r = SUT.Create(new GetDatabaseRequest("theDb"));

            r.RelativeUrl.Should().Be("/");
        }
    }
}