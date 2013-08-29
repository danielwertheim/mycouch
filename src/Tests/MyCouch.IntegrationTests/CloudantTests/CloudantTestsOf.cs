using MyCouch.IntegrationTests.TestFixtures;
using MyCouch.Testing;
using Xunit;

namespace MyCouch.IntegrationTests.CloudantTests
{
    public abstract class CloudantTestsOf<T>
        : TestsOf<T>, IUseFixture<CloudantTestsFixture> where T : class
    {
        protected IClient Client { get; set; }

        protected abstract void OnTestInit();

        public void SetFixture(CloudantTestsFixture data)
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