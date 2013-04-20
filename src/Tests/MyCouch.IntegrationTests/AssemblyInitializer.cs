using NUnit.Framework;

namespace MyCouch.IntegrationTests
{
    [SetUpFixture]
    public class AssemblyInitializer
    {
        [SetUp]
        public void RunBeforeAnyTests()
        {
            //using (var client = TestClientFactory.CreateDefault())
            //{
            //    client.Databases.Put(TestConstants.TestDbName);
            //}
        }

        [TearDown]
        public void RunAfterAnyTests()
        {
            //using (var client = TestClientFactory.CreateDefault())
            //{
            //    client.Databases.Delete(TestConstants.TestDbName);
            //}
        }
    }
}
