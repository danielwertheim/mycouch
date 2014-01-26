using System;
using MyCouch.Cloudant;
using MyCouch.Testing;

namespace MyCouch.IntegrationTests
{
    public abstract class CloudantTestsOf<T> :
        TestsOf<T>,
        IDisposable where T : class
    {
        protected IMyCouchCloudantClient Client { get; set; }

        protected CloudantTestsOf()
        {
            Client = IntegrationTestsRuntime.CreateCloudantClient();
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