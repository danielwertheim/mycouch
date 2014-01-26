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