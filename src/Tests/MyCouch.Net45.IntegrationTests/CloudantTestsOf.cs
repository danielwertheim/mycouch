using System;
using MyCouch.Cloudant;
using MyCouch.Testing;

namespace MyCouch.IntegrationTests
{
    public abstract class CloudantTestsOf<T> :
        TestsOf<T>,
        IDisposable where T : class
    {
        protected readonly TestEnvironment Environment;
        protected IMyCouchCloudantClient Client { get; set; }

        protected CloudantTestsOf() : this(IntegrationTestsRuntime.CloudantClientEnvironment) { }

        protected CloudantTestsOf(TestEnvironment environment)
        {
            Environment = environment;
            Client = IntegrationTestsRuntime.CreateCloudantClient(Environment);
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