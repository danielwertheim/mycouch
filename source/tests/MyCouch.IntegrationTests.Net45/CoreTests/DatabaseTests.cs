using System.Net.Http;
using MyCouch.Testing;
using MyCouch.Testing.TestData;

namespace MyCouch.IntegrationTests.CoreTests
{
    public class DatabaseTests : IntegrationTestsOf<IDatabase>
    {
        public DatabaseTests()
        {
            SUT = DbClient.Database;
            SUT.PutAsync().Wait();
        }

        [MyFact(TestScenarios.DatabaseContext)]
        public void When_Head_of_existing_db_The_response_should_be_200()
        {
            var response = SUT.HeadAsync().Result;

            response.Should().Be(HttpMethod.Head);
        }

        [MyFact(TestScenarios.DatabaseContext)]
        public void When_Get_of_existing_db_with_insert_update_and_delete_ops_The_response_should_be_200()
        {
            var a1 = DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var a1Updated = DbClient.Documents.PutAsync(a1.Id, a1.Rev, ClientTestData.Artists.Artist1Json).Result;

            var a2 = DbClient.Documents.PostAsync(ClientTestData.Artists.Artist2Json).Result;
            var a2Deleted = DbClient.Documents.DeleteAsync(a2.Id, a2.Rev).Result;

            var response = SUT.GetAsync().Result;

            response.Should().BeSuccessful(DbClient.Connection.DbName);
        }

        [MyFact(TestScenarios.DatabaseContext, TestScenarios.CompactDbs)]
        public void When_Compact_of_existing_db_The_response_should_be_202()
        {
            var response = SUT.CompactAsync().Result;

            response.Should().BeAcceptedPost(DbClient.Connection.DbName);
        }

        [MyFact(TestScenarios.DatabaseContext)]
        public void When_ViewCleanup_and_db_exists_The_response_be()
        {
            var response = SUT.ViewCleanupAsync().Result;

            response.Should().BeAcceptedPost(DbClient.Connection.DbName);
        }
    }
}