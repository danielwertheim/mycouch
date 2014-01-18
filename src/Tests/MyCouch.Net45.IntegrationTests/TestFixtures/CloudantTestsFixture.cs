using System;
using MyCouch.Cloudant;

namespace MyCouch.IntegrationTests.TestFixtures
{
    public class CloudantTestsFixture : IDisposable
    {
        public IMyCouchCloudantClient Client { get; protected set; }

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