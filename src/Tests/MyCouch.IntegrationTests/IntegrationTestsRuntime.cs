using MyCouch.Testing;

namespace MyCouch.IntegrationTests
{
    internal static class IntegrationTestsRuntime
    {
        static IntegrationTestsRuntime()
        {
            using (var client = CreateClient())
            {
                //client.Database.PutAsync().Wait();
                client.ClearAllDocuments();
            }

        }

        internal static IClient CreateClient()
        {
            var uriBuilder = new MyCouchUriBuilder("http://localhost:5984/")
                .SetDbName(TestConstants.TestDbName)
                .SetBasicCredentials("mycouchtester", "p@ssword");

            return new Client(uriBuilder.Build());
        }
    }
}