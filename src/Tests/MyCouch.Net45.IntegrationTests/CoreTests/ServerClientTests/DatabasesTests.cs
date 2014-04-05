using System.Net;
using System.Net.Http;
using MyCouch.Testing;

namespace MyCouch.IntegrationTests.CoreTests.ServerClientTests
{
    public class DatabasesTests : IntegrationTestsOf<IDatabases>
    {
        public DatabasesTests()
        {
            SUT = ServerClient.Databases;
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.CreateDb, TestScenarios.DeleteDb)]
        public void Can_create_and_drop_db()
        {
            SUT.DeleteAsync(Environment.TempDbName).Wait();

            var putResponse = SUT.PutAsync(Environment.TempDbName).Result;
            putResponse.Should().Be(HttpMethod.Put, HttpStatusCode.Created);

            var deleteResponse = SUT.DeleteAsync(Environment.TempDbName).Result;
            deleteResponse.Should().Be(HttpMethod.Delete);
        }

        [MyFact(TestScenarios.DatabasesContext)]
        public void When_Head_of_existing_db_The_response_should_be_200()
        {
            var response = SUT.HeadAsync(DbClient.Connection.DbName).Result;

            response.Should().Be(HttpMethod.Head);
        }

        [MyFact(TestScenarios.DatabasesContext)]
        public void When_Get_of_existing_db_The_response_should_be_200()
        {
            var response = SUT.GetAsync(DbClient.Connection.DbName).Result;

            response.Should().BeAnyJson(HttpMethod.Get);
        }

        [MyFact(TestScenarios.DatabasesContext)]
        public void When_Compact_of_existing_db_The_response_should_be_202()
        {
            var response = SUT.CompactAsync(DbClient.Connection.DbName).Result;

            response.Should().BeOkJson(HttpMethod.Post, HttpStatusCode.Accepted);
        }

        [MyFact(TestScenarios.DatabasesContext)]
        public void When_ViewCleanup_and_db_exists_The_response_be()
        {
            var response = SUT.ViewCleanupAsync(DbClient.Connection.DbName).Result;

            response.Should().BeOkJson(HttpMethod.Post, HttpStatusCode.Accepted);
        }
    }
}