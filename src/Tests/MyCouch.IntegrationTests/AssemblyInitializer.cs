using NUnit.Framework;

namespace MyCouch.IntegrationTests
{
    [SetUpFixture]
    public class AssemblyInitializer
    {
        [SetUp]
        public void RunBeforeAnyTests()
        {
            IntegrationTestsRuntime.Init();
        }

        [TearDown]
        public void RunAfterAnyTests()
        {
            IntegrationTestsRuntime.Close();
        }
    }
}