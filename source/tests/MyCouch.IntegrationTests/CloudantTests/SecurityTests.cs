using MyCouch.Cloudant;
using MyCouch.Testing;
using Xunit;

namespace MyCouch.IntegrationTests.CloudantTests
{
    [Trait("Category", "IntegrationTests.CloudantTests")]
    public class SecurityTests : IntegrationTestsOf<ISecurity>
    {
        public SecurityTests()
        {
            SUT = CloudantServerClient.Security;
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.SecurityContext)]
        public void Can_generate_api_key()
        {
            var response = SUT.GenerateApiKey().Result;

            response.Should().BeSuccessful();
        }
    }
}