using System;
using MyCouch;
using MyCouch.Requests;
using MyCouch.Testing;
using MyCouch.Testing.TestData;

namespace IntegrationTests.CoreTests
{
    public class ReplicatorTests : IntegrationTestsOf<IReplicator>
    {
        private readonly string _sourceDb;
        private readonly string _targetDb;

        public ReplicatorTests()
        {
            SUT = ServerClient.Replicator;

            var serverUrl = new UriBuilder(new Uri(Environment.ServerUrl))
            {
                UserName = IntegrationTestsRuntime.Config.GetUsername(),
                Password = Uri.EscapeDataString(IntegrationTestsRuntime.Config.GetPassword())
            }.ToString();
            _sourceDb = $"{serverUrl}{Environment.PrimaryDbName}";
            _targetDb = $"{serverUrl}{Environment.SecondaryDbName}";
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.Replication)]
        public void When_Replicate_and_no_changes_exists_The_response_indicates_success()
        {
            var id = Guid.NewGuid().ToString("n");
            var response = SUT.ReplicateAsync(id, _sourceDb, _targetDb).Result;

            response.Should().BeSuccessfulReplication(id);
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.Replication)]
        public void When_Replicate_and_changes_exists_The_response_indicates_success()
        {
            var id = Guid.NewGuid().ToString("n");
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json);
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist2Json);

            var response = SUT.ReplicateAsync(id, _sourceDb, _targetDb).Result;

            response.Should().BeSuccessfulReplication(id);
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.Replication)]
        public void When_Replicate_using_proxy_and_changes_exists_The_response_indicates_success()
        {
            var id = Guid.NewGuid().ToString("n");
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json);
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist2Json);

            var request = new ReplicateDatabaseRequest(id, _sourceDb, _targetDb)
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

            var request = new ReplicateDatabaseRequest(id, _sourceDb, _targetDb)
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

            var request = new ReplicateDatabaseRequest(id, _sourceDb, _targetDb)
            {
                Continuous = true
            };

            var response = SUT.ReplicateAsync(request).Result;

            response.Should().BeSuccessfulReplication(id);
        }
    }
}