using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace MyCouch.IntegrationTests
{
    internal static class IntegrationTestsRuntime
    {
        private const string TestEnvironmentsBaseUrl = "http://localhost:8991/testenvironments/";
        private static readonly TestEnvironment LocalEnvironment;

        static IntegrationTestsRuntime()
        {
            using (var c = new HttpClient())
            {
                LocalEnvironment = GetTestEnvironment(c, "local");
            }

            if (LocalEnvironment != null)
            {
                using (var client = CreateClient())
                {
                    //client.Database.PutAsync().Wait();
                    client.ClearAllDocuments();
                }
            }
        }

        private static TestEnvironment GetTestEnvironment(HttpClient client, string envName)
        {
            var r = client.GetAsync(TestEnvironmentsBaseUrl + envName).Result;
            if (r.StatusCode == HttpStatusCode.NotFound)
                return null;

            if(!r.IsSuccessStatusCode)
                throw new Exception("Could not load test environment settings for: " + TestEnvironmentsBaseUrl + envName);

            return JsonConvert.DeserializeObject<TestEnvironment>(r.Content.ReadAsStringAsync().Result);
        }

        internal static IClient CreateClient()
        {
            if(LocalEnvironment == null)
                throw new Exception("Can not create client for Local test environmet. Missing configuration.");

            var cfg = LocalEnvironment.Client;
            var uriBuilder = new MyCouchUriBuilder(cfg.ServerUrl)
                .SetDbName(LocalEnvironment.Client.DbName)
                .SetBasicCredentials(cfg.User, cfg.Password);

            return new Client(uriBuilder.Build());
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