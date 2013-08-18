using System;
using MyCouch.Testing;

namespace MyCouch.IntegrationTests
{
    internal static class IntegrationTestsRuntime
    {
        static IntegrationTestsRuntime()
        {
            using (var client = CreateClient())
            {
                //client.Database.Put();
                client.ClearAllDocuments();
            }

        }
        internal static IClient CreateClient()
        {
            return new Client("http://mycouchtester:" + Uri.EscapeDataString("p@ssword") + "@localhost:5984/" + TestConstants.TestDbName + "/");
        }
    }
}