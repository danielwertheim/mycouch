using MyCouch.Cloudant;

namespace MyCouch.IntegrationTests
{
    internal static class IntegrationTestsRuntime
    {
        private const string ServerUrl = "http://localhost:5984";
        private const string TesterAccount = "mycouchtester";
        private const string TesterPassword = "p@ssword";

        static IntegrationTestsRuntime()
        {
            using (var client = CreateClient())
            {
                //client.Database.PutAsync().Wait();
                client.ClearAllDocuments();
            }

            using (var client = CreateCloudantClient())
            {
                //client.Database.PutAsync().Wait();
                client.ClearAllDocuments();
            }
        }

        internal static IClient CreateClient()
        {
            var uriBuilder = new MyCouchUriBuilder(ServerUrl)
                .SetDbName("mycouchtests")
                .SetBasicCredentials(TesterAccount, TesterPassword);

            return new Client(uriBuilder.Build());
        }

        internal static ICloudantClient CreateCloudantClient()
        {
            var uriBuilder = new MyCouchUriBuilder(ServerUrl)
                .SetDbName("mycouchtests")
                .SetBasicCredentials(TesterAccount, TesterPassword);

            return new CloudantClient(uriBuilder.Build());
        }
    }
}