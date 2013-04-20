using MyCouch.Testing;
using NUnit.Framework;

namespace MyCouch.IntegrationTests
{
    [TestFixture]
    public abstract class IntegrationTestsOf<T> : TestsOf<T> where T : class
    {
        protected IClient Client;

        protected override void OnFixtureInitialize()
        {
            base.OnFixtureInitialize();

            Client = TestClientFactory.CreateDefault();
        }

        protected override void OnFixtureFinalize()
        {
            base.OnFixtureFinalize();

            Client.Dispose();
            Client = null;
        }
    }

    [TestFixture]
    public abstract class IntegrationTestsOf : TestsOf { }
}