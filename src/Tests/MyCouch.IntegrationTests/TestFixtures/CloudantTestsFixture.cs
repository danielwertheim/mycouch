using System;

namespace MyCouch.IntegrationTests.TestFixtures
{
    public class CloudantTestsFixture : IDisposable
    {
        public IClient Client { get; protected set; }

        public CloudantTestsFixture()
        {
            Client = IntegrationTestsRuntime.CreateCloudantClient();
        }

        public virtual void Dispose()
        {
            Client.Dispose();
            Client = null;
        }
    }
}