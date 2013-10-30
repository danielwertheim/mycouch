using System;
using System.Net;
using System.Net.Http;
using MyCouch.Cloudant;
using Newtonsoft.Json;

namespace MyCouch.IntegrationTests
{
    internal static class IntegrationTestsRuntime
    {
        private const string TestEnvironmentsBaseUrl = "http://localhost:8991/testenvironments/";
        private static readonly TestEnvironment NormalEnvironment;
        private static readonly TestEnvironment CloudantEnvironment;

        static IntegrationTestsRuntime()
        {
            using (var c = new HttpClient())
            {
                NormalEnvironment = GetTestEnvironment(c, "normal");
                CloudantEnvironment = GetTestEnvironment(c, "cloudant");
            }

            if (NormalEnvironment != null)
            {
                using (var client = CreateNormalClient())
                {
                    //client.Database.PutAsync().Wait();
                    client.ClearAllDocuments();
                }
            }

            if (CloudantEnvironment != null)
            {
                using (var client = CreateCloudantClient())
                {
                    //client.Database.PutAsync().Wait();
                    client.ClearAllDocuments();
                }
            }
        }

        private static TestEnvironment GetTestEnvironment(HttpClient client, string envName)
        {
            try
            {
                var r = client.GetAsync(TestEnvironmentsBaseUrl + envName).Result;
                if (r.StatusCode == HttpStatusCode.NotFound)
                    return null;

                if (!r.IsSuccessStatusCode)
                    throw new Exception("Could not load test environment settings for: " + TestEnvironmentsBaseUrl + envName);

                return JsonConvert.DeserializeObject<TestEnvironment>(r.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                throw new AggregateException("Could not load test environment settings for: " + TestEnvironmentsBaseUrl + envName, ex);
            }
        }

        internal static IClient CreateNormalClient()
        {
            if(NormalEnvironment == null)
                throw new Exception("Can not create client for Normal tests. Missing configuration.");

            var cfg = NormalEnvironment.Client;
            var uriBuilder = new MyCouchUriBuilder(cfg.ServerUrl)
                .SetDbName(NormalEnvironment.Client.DbName)
                .SetBasicCredentials(cfg.User, cfg.Password);

            return new Client(uriBuilder.Build());
        }

        internal static ICloudantClient CreateCloudantClient()
        {
            if (NormalEnvironment == null)
                throw new Exception("Can not create client for Cloudant tests. Missing configuration.");

            var cfg = CloudantEnvironment.Client;
            var uriBuilder = new MyCouchUriBuilder(cfg.ServerUrl)
                .SetDbName(NormalEnvironment.Client.DbName)
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