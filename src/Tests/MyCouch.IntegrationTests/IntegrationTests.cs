using MyCouch.Testing;

namespace MyCouch.IntegrationTests
{
    public abstract class IntegrationTests<TClient, TToTest> : TestsOf<TToTest> where TClient : class, IClient where TToTest : class
    {
        protected TClient Client { get; private set; }

        protected IntegrationTests(TClient client)
        {
            Client = client;
        }

        public override void Dispose()
        {
            base.Dispose();

            if(!(this is IPreserveStatePerFixture)) 
                Client.ClearAllDocuments();

            Client.Dispose();
            Client = null;
        }
    }
}