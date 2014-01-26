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
            CleanDb();
        }

        public virtual void Dispose()
        {
            CleanDb();
            Client.Dispose();
            Client = null;
        }

        protected void CleanDb()
        {
            if (!(this is IPreserveStatePerFixture))
                Client.ClearAllDocuments();
        }
    }
}