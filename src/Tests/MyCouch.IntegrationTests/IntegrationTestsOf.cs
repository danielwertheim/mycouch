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

            Client = IntegrationTestsRuntime.Client;
        }
    }

    [TestFixture]
    public abstract class IntegrationTestsOf : TestsOf { }
}