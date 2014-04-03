using System;
using MyCouch.Testing;

namespace MyCouch.IntegrationTests
{
    public abstract class ServerClientTestsOf<T> :
        TestsOf<T>,
        IDisposable where T : class
    {
        protected readonly TestEnvironment Environment;
        protected IMyCouchClient Client { get; set; }
        protected IMyCouchServerClient ServerClient { get; set; }

        protected ServerClientTestsOf()
        {
            Environment = IntegrationTestsRuntime.NormalEnvironment;
            Client = IntegrationTestsRuntime.CreateDbClient(Environment);
            ServerClient = IntegrationTestsRuntime.CreateServerClient(Environment);
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

            var disposableSut = SUT as IDisposable;
            if(disposableSut == null)
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