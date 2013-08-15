using MyCouch.Testing;
#if !NETFX_CORE
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif
using MyCouch.Extensions;

namespace MyCouch.IntegrationTests
{
    [TestClass]
    public abstract class IntegrationTestsOf<T> : TestsOf<T> where T : class
    {
        protected IClient Client;

        protected IntegrationTestsOf()
        {
            Client = IntegrationTestsRuntime.Client;
        }
    }

    [TestClass]
    public abstract class IntegrationTestsOf : TestsOf { }
}