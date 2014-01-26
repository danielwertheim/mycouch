using System;
using MyCouch.Testing;

namespace MyCouch.IntegrationTests
{
    public abstract class ClientTestsOf<T> :
        TestsOf<T>,
        IDisposable where T : class
    {
        protected IMyCouchClient Client { get; set; }

        protected ClientTestsOf()
        {
            Client = IntegrationTestsRuntime.CreateNormalClient();
        }

        public virtual void Dispose()
        {
            if (!(this is IPreserveStatePerFixture))
                Client.ClearAllDocuments();

            Client.Dispose();
            Client = null;
        }
    }
}