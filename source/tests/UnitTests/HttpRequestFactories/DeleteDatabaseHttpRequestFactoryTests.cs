using FluentAssertions;
using MyCouch.HttpRequestFactories;
using MyCouch.Requests;
using Xunit;

namespace UnitTests.HttpRequestFactories
{
    public class DeleteDatabaseHttpRequestFactoryTests : UnitTestsOf<DeleteDatabaseHttpRequestFactory>
    {
        [Fact]
        public void When_passing_db_name_It_should_generate_a_relative_url()
        {
            SUT = new DeleteDatabaseHttpRequestFactory();

            var r = SUT.Create(new DeleteDatabaseRequest("theDb"));

            r.RelativeUrl.Should().Be("/");
        }
    }
}