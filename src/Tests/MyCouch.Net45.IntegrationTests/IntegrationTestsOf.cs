using System;
using MyCouch.Cloudant;
using MyCouch.Testing;

namespace MyCouch.IntegrationTests
{
    public abstract class IntegrationTestsOf<T> :
        TestsOf<T>,
        IDisposable where T : class
    {
        protected readonly TestEnvironment Environment;
        protected IMyCouchServerClient ServerClient { get; set; }
        protected IMyCouchClient DbClient { get; set; }
        protected IMyCouchCloudantClient CloudantDbClient { get; set; }

        protected IntegrationTestsOf()
        {
            Environment = IntegrationTestsRuntime.Environment;
            ServerClient = IntegrationTestsRuntime.CreateServerClient(Environment);
            DbClient = IntegrationTestsRuntime.CreateDbClient(Environment);
            CloudantDbClient = IntegrationTestsRuntime.CreateCloudantDbClient(Environment);

            CleanDb();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!disposing)
                return;

            CleanDb();

            ServerClient.Dispose();
            ServerClient = null;

            DbClient.Dispose();
            DbClient = null;

            CloudantDbClient.Dispose();
            CloudantDbClient = null;

            var disposableSut = SUT as IDisposable;
            if(disposableSut == null)
                return;

            disposableSut.Dispose();
        }

        protected void CleanDb()
        {
            if (!(this is IPreserveStatePerFixture))
                DbClient.ClearAllDocuments();
        }
    }
}