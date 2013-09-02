using System.Net.Http;
using MyCouch.Cloudant;
using Newtonsoft.Json;

namespace MyCouch.IntegrationTests
{
    internal static class IntegrationTestsRuntime
    {
        private const string TestEnvironmentsBaseUrl = "http://localhost:8991/testenvironments/";
        private static readonly TestEnvironment LocalEnvironment;
        private static readonly TestEnvironment CloudantEnvironment;

        static IntegrationTestsRuntime()
        {
            using (var c = new HttpClient())
            {
                LocalEnvironment = JsonConvert.DeserializeObject<TestEnvironment>(
                    c.GetStringAsync(TestEnvironmentsBaseUrl + "local").Result);

                CloudantEnvironment = JsonConvert.DeserializeObject<TestEnvironment>(
                    c.GetStringAsync(TestEnvironmentsBaseUrl + "cloudant").Result);
            }

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
            var cfg = LocalEnvironment.Client;
            var uriBuilder = new MyCouchUriBuilder(cfg.ServerUrl)
                .SetDbName(LocalEnvironment.Client.DbName)
                .SetBasicCredentials(cfg.User, cfg.Password);

            return new Client(uriBuilder.Build());
        }

        internal static ICloudantClient CreateCloudantClient()
        {
            var cfg = CloudantEnvironment.Client;
            var uriBuilder = new MyCouchUriBuilder(cfg.ServerUrl)
                .SetDbName(LocalEnvironment.Client.DbName)
                .SetBasicCredentials(cfg.User, cfg.Password);

            return new CloudantClient(uriBuilder.Build());
        }

        private class TestEnvironment
        {
            public ClientConfig Client { get; set; }

            public TestEnvironment()
            {
                Client = new ClientConfig();
            }
        }
        private class ClientConfig
        {
            public string ServerUrl { get; set; }
            public string DbName { get; set; }
            public string User { get; set; }
            public string Password { get; set; }
        }
    }
}