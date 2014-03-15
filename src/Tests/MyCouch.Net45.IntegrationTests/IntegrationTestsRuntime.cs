using System;
using System.Net;
using System.Net.Http;
using MyCouch.Cloudant;
using MyCouch.Net;
using Newtonsoft.Json;

namespace MyCouch.IntegrationTests
{
    internal static class IntegrationTestsRuntime
    {
        private const string TestEnvironmentsBaseUrl = "http://localhost:8991/testenvironments/";

        internal static readonly TestEnvironment CoreEnvironment;
        internal static readonly TestEnvironment TempClientEnvironment;
        internal static readonly TestEnvironment CloudantClientEnvironment;

        static IntegrationTestsRuntime()
        {
            using (var c = new HttpClient())
            {
                CoreEnvironment = GetTestEnvironment(c, "normal");
                TempClientEnvironment = GetTestEnvironment(c, "temp");
                CloudantClientEnvironment = GetTestEnvironment(c, "cloudant");
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

        internal static IMyCouchClient CreateClient(TestEnvironment environment)
        {
            var config = environment.Client;
            var uriBuilder = new MyCouchUriBuilder(config.ServerUrl)
                .SetDbName(config.DbName);

            if(config.HasCredentials())
                uriBuilder.SetBasicCredentials(config.User, config.Password);

            return config.IsAgainstCloudant()
                ? new MyCouchClient(new CustomCloudantConnection(uriBuilder.Build()))
                : new MyCouchClient(uriBuilder.Build());
        }

        internal static IMyCouchCloudantClient CreateCloudantClient(TestEnvironment environment)
        {
            if(!CloudantClientEnvironment.Client.IsAgainstCloudant())
                throw new Exception("The configuration for the Cloudant test environment does not seem to point to a Cloudant Db.");

            var cfg = CloudantClientEnvironment.Client;
            var uriBuilder = new MyCouchUriBuilder(cfg.ServerUrl)
                .SetDbName(cfg.DbName)
                .SetBasicCredentials(cfg.User, cfg.Password);

            return new MyCouchCloudantClient(new CustomCloudantConnection(uriBuilder.Build()));
        }

        private class CustomCloudantConnection : BasicHttpClientConnection
        {
            public CustomCloudantConnection(Uri uri)
                : base(uri) { }

            protected override HttpRequest OnBeforeSend(HttpRequest httpRequest)
            {
                if (httpRequest.Method == HttpMethod.Post || httpRequest.Method == HttpMethod.Put || httpRequest.Method == HttpMethod.Delete)
                {
                    httpRequest.RequestUri = string.IsNullOrEmpty(httpRequest.RequestUri.Query)
                        ? new Uri(httpRequest.RequestUri + "?w=3")
                        : new Uri(httpRequest.RequestUri + "&w=3");
                }

                if (httpRequest.Method == HttpMethod.Get || httpRequest.Method == HttpMethod.Head)
                {
                    httpRequest.RequestUri = string.IsNullOrEmpty(httpRequest.RequestUri.Query)
                        ? new Uri(httpRequest.RequestUri + "?r=1")
                        : new Uri(httpRequest.RequestUri + "&r=1");
                }
                return base.OnBeforeSend(httpRequest);
            }
        }
    }

    public class TestEnvironment
    {
        public ClientConfig Client { get; set; }
        public bool SupportsDatabaseContextTests { get; set; }

        public TestEnvironment()
        {
            Client = new ClientConfig();
            SupportsDatabaseContextTests = false;
        }
    }

    public class ClientConfig
    {
        public string ServerUrl { get; set; }
        public string DbName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public bool IsAgainstCloudant()
        {
            return ServerUrl.Contains("cloudant.com");
        }

        public bool HasCredentials()
        {
            return !string.IsNullOrEmpty(User);
        }
    }
}