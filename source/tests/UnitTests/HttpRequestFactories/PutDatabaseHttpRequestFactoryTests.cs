using FluentAssertions;
using MyCouch.HttpRequestFactories;
using MyCouch.Requests;
using Xunit;

namespace UnitTests.HttpRequestFactories
{
    public class PutDatabaseHttpRequestFactoryTests : UnitTestsOf<PutDatabaseHttpRequestFactory>
    {
        [Fact]
        public void When_passing_db_name_It_should_generate_a_relative_url()
        {
            SUT = new PutDatabaseHttpRequestFactory();

            var r = SUT.Create(new PutDatabaseRequest("theDb"));

            r.RelativeUrl.Should().Be("/");
        }
    }
}