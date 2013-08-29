namespace MyCouch.IntegrationTests
{
    internal static class IntegrationTestsRuntime
    {
        private const string ServerUrl = "http://localhost:5984";

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
            var uriBuilder = new MyCouchUriBuilder(ServerUrl)
                .SetDbName("mycouchtests")
                .SetBasicCredentials("mycouchtester", "p@ssword");

            return new Client(uriBuilder.Build());
        }

        internal static IClient CreateCloudantClient()
        {
            var uriBuilder = new MyCouchUriBuilder(ServerUrl)
                .SetDbName("mycouchcloudanttests")
                .SetBasicCredentials("mycouchtester", "p@ssword");

            return new Client(uriBuilder.Build());
        }
    }
}