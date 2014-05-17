using System;
using MyCouch.Requests;
using MyCouch.Testing;
using MyCouch.Testing.TestData;

namespace MyCouch.IntegrationTests.CoreTests
{
    public class ReplicatorTests : IntegrationTestsOf<IReplicator>
    {
        public ReplicatorTests()
        {
            SUT = ServerClient.Replicator;
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.Replication)]
        public void When_Replicate_and_no_changes_exists_The_response_indicates_success()
        {
            var id = Guid.NewGuid().ToString("n");
            var response = SUT.ReplicateAsync(id, Environment.PrimaryDbName, Environment.SecondaryDbName).Result;

            response.Should().BeSuccessfulReplication(id);
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.Replication)]
        public void When_Replicate_and_changes_exists_The_response_indicates_success()
        {
            var id = Guid.NewGuid().ToString("n");
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json);
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist2Json);

            var response = SUT.ReplicateAsync(id, Environment.PrimaryDbName, Environment.SecondaryDbName).Result;

            response.Should().BeSuccessfulReplication(id);
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.Replication)]
        public void When_Replicate_using_proxy_and_changes_exists_The_response_indicates_success()
        {
            var id = Guid.NewGuid().ToString("n");
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json);
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist2Json);

            var request = new ReplicateDatabaseRequest(id, Environment.PrimaryDbName, Environment.SecondaryDbName)
            {
                Proxy = Environment.ServerUrl
            };

            var response = SUT.ReplicateAsync(request).Result;

            response.Should().BeSuccessfulReplication(id);
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.Replication)]
        public void When_Replicate_using_doc_ids_and_changes_exists_The_response_indicates_success()
        {
            var id = Guid.NewGuid().ToString("n");
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json);
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist2Json);

            var request = new ReplicateDatabaseRequest(id, Environment.PrimaryDbName, Environment.SecondaryDbName)
            {
                DocIds = new[] { ClientTestData.Artists.Artist1Id }
            };

            var response = SUT.ReplicateAsync(request).Result;

            response.Should().BeSuccessfulReplication(id);
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.Replication)]
        public void Can_do_continuous_replication()
        {
            var id = Guid.NewGuid().ToString("n");
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json);
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist2Json);

            var request = new ReplicateDatabaseRequest(id, Environment.PrimaryDbName, Environment.SecondaryDbName)
            {
                Continuous = true
            };

            var response = SUT.ReplicateAsync(request).Result;

            response.Should().BeSuccessfulReplication(id);
        }
    }
}