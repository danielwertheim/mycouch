using System;
using MyCouch.Requests;
using Xunit;

namespace MyCouch.IntegrationTests.CoreTests.ServerClientTests
{
    public class ReplicationTests : ServerClientTestsOf<IReplication>
    {
        [Fact]
        public void When_Replicate_between_existing_dbs_The_response_indicates_success()
        {
            throw new NotImplementedException();
            //var request = new ReplicateDatabaseRequest("8bb3dae14ee846f388fab59e738da343", "6178485ca006446ebfdb4bc2667325a5");

            //var response = SUT.ReplicateAsync(request);
        }
    }
}