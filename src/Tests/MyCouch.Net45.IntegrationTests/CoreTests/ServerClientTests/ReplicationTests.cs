using Xunit;

namespace MyCouch.IntegrationTests.CoreTests.ServerClientTests
{
    public class ReplicationTests : ServerClientTestsOf<IReplication>
    {
        [Fact]
        public void When_Replicate_between_existing_dbs_The_response_indicates_success()
        {
        }
    }
}