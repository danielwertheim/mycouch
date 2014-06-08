using FluentAssertions;
using MyCouch.Cloudant.HttpRequestFactories;
using MyCouch.Cloudant.Requests;
using Xunit;

namespace MyCouch.UnitTests.Cloudant.HttpRequestFactories
{
    public class GenerateApiKeyHttpRequestFactoryTests : UnitTestsOf<GenerateApiKeyHttpRequestFactory>
    {
        public GenerateApiKeyHttpRequestFactoryTests()
        {
            SUT = new GenerateApiKeyHttpRequestFactory();
        }

        [Fact]
        public void It_shall_generate_an_relative_uri()
        {
            var httpRequest = SUT.Create(new GenerateApiKeyRequest());

            httpRequest.RelativeUrl.Should().Be("/_api/v2/api_keys");
        }
    }
}