namespace MyCouch.IntegrationTests
{
    internal static class TestClientFactory
    {
        internal static IClient CreateDefault()
        {
            return new Client("http://localhost:5984/" + TestConstants.TestDbName);
        }
    }
}