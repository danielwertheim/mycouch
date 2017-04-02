using System;
using MyCouch.Testing;
using Xunit;

namespace MyCouch.IntegrationTests
{
    [Collection("Integration tests")]
    public abstract class IntegrationTestsOf<T> :
        TestsOf<T>,
        IDisposable where T : class
    {
        protected readonly TestEnvironment Environment;
        protected IMyCouchServerClient ServerClient { get; private set; }
        protected IMyCouchClient DbClient { get; private set; }

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

            disposableSut?.Dispose();
        }

        private void EnsureCleanEnvironment()
        {
            if (!(this is IPreserveStatePerFixture))
                IntegrationTestsRuntime.EnsureCleanEnvironment();
        }
    }
}