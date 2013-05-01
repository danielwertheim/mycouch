namespace MyCouch.IntegrationTests
{
    internal static class TestClientFactory
    {
        internal static IClient CreateDefault()
        {
            return new Client("http://mycouchtester:1q2w3e4r@localhost:5984/" + TestConstants.TestDbName);
        }
    }
}