using System;

namespace MyCouch.IntegrationTests
{
    internal static class TestClientFactory
    {
        internal static IClient CreateDefault()
        {
            return new Client("http://mycouchtester:" + Uri.EscapeDataString("p@ssword") + "@localhost:5984/" + TestConstants.TestDbName);
        }
    }
}