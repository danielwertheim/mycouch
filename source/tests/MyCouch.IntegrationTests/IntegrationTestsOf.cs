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
        protected IMyCouchServerClient ServerClient { get; private set; }
        protected IMyCouchClient DbClient { get; private set; }

        protected IMyCouchCloudantServerClient CloudantServerClient
        {
            get
            {
                return ServerClient as IMyCouchCloudantServerClient;
            }
        }

        protected IMyCouchCloudantClient CloudantDbClient
        {
            get
            {
                return DbClient as IMyCouchCloudantClient;
            }
        }

        protected IntegrationTestsOf()
        {
            Environment = IntegrationTestsRuntime.Environment;
            ServerClient = IntegrationTestsRuntime.CreateServerClient();
            DbClient = IntegrationTestsRuntime.CreateDbClient();

            EnsureCleanEnvironment();
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

            ServerClient.Dispose();
            ServerClient = null;

            DbClient.Dispose();
            DbClient = null;

            var disposableSut = SUT as IDisposable;
            if (disposableSut == null)
                return;

            disposableSut.Dispose();
        }

        protected void EnsureCleanEnvironment()
        {
            if (!(this is IPreserveStatePerFixture))
                IntegrationTestsRuntime.EnsureCleanEnvironment();
        }
    }
}