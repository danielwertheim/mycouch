using MyCouch.Testing;

namespace MyCouch.IntegrationTests
{
    public abstract class IntegrationTestsOf<T> : TestsOf<T> where T : class
    {
        protected IClient Client { get; private set; }

        protected IntegrationTestsOf()
        {
            Client = IntegrationTestsRuntime.CreateClient();
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