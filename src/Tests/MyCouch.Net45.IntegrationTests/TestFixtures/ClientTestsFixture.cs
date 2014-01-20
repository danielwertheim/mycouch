using System;

namespace MyCouch.IntegrationTests.TestFixtures
{
    public class ClientTestsFixture : IDisposable
    {
        public IMyCouchClient Client { get; protected set; }

        public ClientTestsFixture()
        {
            Client = IntegrationTestsRuntime.CreateNormalClient();
        }

        public virtual void Dispose()
        {
            Client.Dispose();
            Client = null;
        }
    }
}