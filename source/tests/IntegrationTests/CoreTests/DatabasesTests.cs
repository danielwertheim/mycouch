﻿using System.Net.Http;
using MyCouch;
using MyCouch.Testing;
using MyCouch.Testing.TestData;

namespace IntegrationTests.CoreTests
{
    public class DatabasesTests : IntegrationTestsOf<IDatabases>
    {
        public DatabasesTests()
        {
            SUT = ServerClient.Databases;
        }

        [MyFact(TestScenarios.DatabasesContext)]
        public void When_Head_of_existing_db_The_response_should_be_200()
        {
            var response = SUT.HeadAsync(Environment.PrimaryDbName).Result;

            response.Should().Be(HttpMethod.Head);
        }

        [MyFact(TestScenarios.DatabasesContext)]
        public void When_Get_of_existing_db_with_insert_update_and_delete_ops_The_response_should_be_200()
        {
            var a1 = DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var a1Updated = DbClient.Documents.PutAsync(a1.Id, a1.Rev, ClientTestData.Artists.Artist1Json).Result;

            var a2 = DbClient.Documents.PostAsync(ClientTestData.Artists.Artist2Json).Result;
            var a2Deleted = DbClient.Documents.DeleteAsync(a2.Id, a2.Rev).Result;

            var response = SUT.GetAsync(Environment.PrimaryDbName).Result;

            response.Should().BeSuccessful(Environment.PrimaryDbName);
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.CompactDbs)]
        public void When_Compact_of_existing_db_The_response_should_be_202()
        {
            var response = SUT.CompactAsync(Environment.TempDbName).Result;

            response.Should().BeAcceptedPost(Environment.TempDbName);
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.ViewCleanUp)]
        public void When_ViewCleanup_and_db_exists_The_response_be()
        {
            var response = SUT.ViewCleanupAsync(Environment.TempDbName).Result;

            response.Should().BeAcceptedPost(Environment.TempDbName);
        }
    }
}