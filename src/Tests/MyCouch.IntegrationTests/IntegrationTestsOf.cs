using MyCouch.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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