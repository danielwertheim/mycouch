using System;

namespace MyCouch.IntegrationTests
{
    internal static class TestClientFactory
    {
        internal static IClient CreateDefault()
        {
            return new Client("http://mycouchtester:" + Uri.EscapeDataString("p@ssword") + "@192.168.33.50:5984/" + TestConstants.TestDbName + "/");
        }
    }
}