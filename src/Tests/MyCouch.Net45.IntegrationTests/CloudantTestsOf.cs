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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            CleanDb();
            Client.Dispose();
            Client = null;

            var disposableSut = SUT as IDisposable;
            if (disposableSut == null)
                return;

            disposableSut.Dispose();
        }

        protected void CleanDb()
        {
            if (!(this is IPreserveStatePerFixture))
                Client.ClearAllDocuments();
        }
    }
}