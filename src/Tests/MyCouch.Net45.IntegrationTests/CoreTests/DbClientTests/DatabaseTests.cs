using System.Net;
using System.Net.Http;
using MyCouch.Testing;

namespace MyCouch.IntegrationTests.CoreTests.DbClientTests
{
    public class DatabaseTests : IntegrationTestsOf<IDatabase>
    {
        public DatabaseTests()
        {
            SUT = DbClient.Database;
            SUT.PutAsync().Wait();
        }

        [MyFact(TestScenarios.DatabasesContext)]
        public void When_Head_of_existing_db_The_response_should_be_200()
        {
            var response = SUT.HeadAsync().Result;

            response.Should().Be(HttpMethod.Head);
        }

        [MyFact(TestScenarios.DatabasesContext)]
        public void When_Get_of_existing_db_The_response_should_be_200()
        {
            var response = SUT.GetAsync().Result;

            response.Should().BeAnyJson(HttpMethod.Get);
        }

        [MyFact(TestScenarios.DatabasesContext)]
        public void When_Compact_of_existing_db_The_response_should_be_202()
        {
            var response = SUT.CompactAsync().Result;

            response.Should().BeOkJson(HttpMethod.Post, HttpStatusCode.Accepted);
        }

        [MyFact(TestScenarios.DatabasesContext)]
        public void When_ViewCleanup_and_db_exists_The_response_be()
        {
            var response = SUT.ViewCleanupAsync().Result;

            response.Should().BeOkJson(HttpMethod.Post, HttpStatusCode.Accepted);
        }
    }
}