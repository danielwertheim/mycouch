using MyCouch.IntegrationTests.TestFixtures;
using MyCouch.Testing;
using Xunit;

namespace MyCouch.IntegrationTests
{
    public abstract class ClientTestsOf<T> : TestsOf<T>, IUseFixture<ClientTestsFixture> where T : class
    {
        protected IMyCouchClient Client { get; set; }

        protected abstract void OnTestInit();

        public void SetFixture(ClientTestsFixture data)
        {
            Client = data.Client;
            OnTestInit();
        }

        public override void Dispose()
        {
            base.Dispose();

            if (!(this is IPreserveStatePerFixture))
                Client.ClearAllDocuments();
        }
    }
}