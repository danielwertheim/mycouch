using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using MyCouch.Testing;
using MyCouch.Testing.TestData;

namespace MyCouch.IntegrationTests.CoreTests.ServerClientTests
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
        public void When_Get_of_existing_db_The_response_should_be_200()
        {
            var response = SUT.GetAsync(Environment.PrimaryDbName).Result;

            response.Should().BeAnyJson(HttpMethod.Get);
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.CompactDbs)]
        public void When_Compact_of_existing_db_The_response_should_be_202()
        {
            var response = SUT.CompactAsync(Environment.TempDbName).Result;

            response.Should().BeOkJson(HttpMethod.Post, HttpStatusCode.Accepted);
        }

        [MyFact(TestScenarios.DatabasesContext)]
        public void When_ViewCleanup_and_db_exists_The_response_be()
        {
            var response = SUT.ViewCleanupAsync(Environment.TempDbName).Result;

            response.Should().BeOkJson(HttpMethod.Post, HttpStatusCode.Accepted);
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.Replication)]
        public void When_Replicate_and_no_changes_exists_The_response_indicates_success()
        {
            var response = SUT.ReplicateAsync(Environment.PrimaryDbName, Environment.SecondaryDbName).Result;

            response.Should().BeSuccessfulButEmptyReplication();
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.Replication)]
        public void When_Replicate_and_changes_exists_The_response_indicates_success()
        {
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json);

            var response = SUT.ReplicateAsync(Environment.PrimaryDbName, Environment.SecondaryDbName).Result;

            response.Should().BeSuccessfulNonEmptyReplication();
        }
    }
}