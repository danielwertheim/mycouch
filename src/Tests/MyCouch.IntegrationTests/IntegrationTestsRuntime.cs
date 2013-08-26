#if !NETFX_CORE
using MyCouch.Configurations;
#endif
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
#if !NETFX_CORE
            var url = ConnectionString.Get("mycouchtests");
#else
            var url = "http://localhost:5984";
#endif
            var uriBuilder = new MyCouchUriBuilder(url)
                .SetDbName(TestConstants.TestDbName)
                .SetBasicCredentials("mycouchtester", "p@ssword");

            return new Client(uriBuilder.Build());
        }
    }
}