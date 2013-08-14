#if !WinRT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

namespace MyCouch.IntegrationTests
{
    [TestClass]
    public static class AssemblyInitializer
    {
        [AssemblyInitialize]
        public static void RunBeforeAnyTests(TestContext context)
        {
            IntegrationTestsRuntime.Init();
        }

        [AssemblyCleanup]
        public static void RunAfterAnyTests()
        {
            IntegrationTestsRuntime.Close();
        }
    }
}