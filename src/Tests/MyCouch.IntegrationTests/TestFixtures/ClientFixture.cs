using System;

namespace MyCouch.IntegrationTests.TestFixtures
{
    public class ClientFixture : IDisposable
    {
        public IClient Client { get; protected set; }

        public ClientFixture()
        {
            Client = IntegrationTestsRuntime.CreateClient();
        }

        public virtual void Dispose()
        {
            Client.Dispose();
            Client = null;
        }
    }
}