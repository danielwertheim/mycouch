using System;

namespace MyCouch.IntegrationTests.TestFixtures
{
    public class ClientTestsFixture : IDisposable
    {
        public IClient Client { get; protected set; }

        public ClientTestsFixture()
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