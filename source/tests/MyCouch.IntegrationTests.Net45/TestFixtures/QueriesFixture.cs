using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCouch.IntegrationTests.TestFixtures
{
    public class QueriesFixture : IDisposable
    {
        internal void Init(TestEnvironment Environment)
        {
            IntegrationTestsRuntime.EnsureCleanEnvironment();
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

            IntegrationTestsRuntime.EnsureCleanEnvironment();
        }
    }
}
