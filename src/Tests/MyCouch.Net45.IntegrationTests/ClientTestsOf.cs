using System;
using MyCouch.Testing;

namespace MyCouch.IntegrationTests
{
    public abstract class ClientTestsOf<T> :
        TestsOf<T>,
        IDisposable where T : class
    {
        protected readonly TestEnvironment Environment;
        protected IMyCouchClient Client { get; set; }

        protected ClientTestsOf() : this(IntegrationTestsRuntime.ClientEnvironment) { }

        protected ClientTestsOf(TestEnvironment environment)
        {
            Environment = environment;
            Client = IntegrationTestsRuntime.CreateClient(Environment);
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