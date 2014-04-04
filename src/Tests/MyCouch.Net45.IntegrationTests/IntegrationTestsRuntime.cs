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

        internal static readonly TestEnvironment NormalEnvironment;
        internal static readonly TestEnvironment TempEnvironment;
        internal static readonly TestEnvironment CloudantEnvironment;

        static IntegrationTestsRuntime()
        {
            using (var c = new HttpClient())
            {
                NormalEnvironment = GetTestEnvironment(c, "normal");
                TempEnvironment = GetTestEnvironment(c, "temp");
                CloudantEnvironment = GetTestEnvironment(c, "cloudant");
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

        internal static IMyCouchServerClient CreateServerClient(TestEnvironment environment)
        {
            if (!environment.RunServerClientTests)
                throw new Exception("The Test environment configuration is not configured to run ServerClient tests.");

            var config = environment.ServerClient;
            var uriBuilder = new MyCouchUriBuilder(config.Url);

            if (config.HasCredentials())
                uriBuilder.SetBasicCredentials(config.User, config.Password);

            return config.IsAgainstCloudant()
                ? new MyCouchServerClient(new CustomCloudantDbClientConnection(uriBuilder.Build()))
                : new MyCouchServerClient(uriBuilder.Build());
        }

        internal static IMyCouchClient CreateDbClient(TestEnvironment environment)
        {
            var config = environment.DbClient;
            var uriBuilder = new MyCouchUriBuilder(config.ServerUrl)
                .SetDbName(config.DbName);

            if(config.HasCredentials())
                uriBuilder.SetBasicCredentials(config.User, config.Password);

            return config.IsAgainstCloudant()
                ? new MyCouchClient(new CustomCloudantDbClientConnection(uriBuilder.Build()))
                : new MyCouchClient(uriBuilder.Build());
        }

        internal static IMyCouchCloudantClient CreateCloudantDbClient(TestEnvironment environment)
        {
            if(!CloudantEnvironment.DbClient.IsAgainstCloudant())
                throw new Exception("The configuration for the Cloudant test environment for the DbClient does not seem to point to a Cloudant Db.");

            var cfg = CloudantEnvironment.DbClient;
            var uriBuilder = new MyCouchUriBuilder(cfg.ServerUrl)
                .SetDbName(cfg.DbName)
                .SetBasicCredentials(cfg.User, cfg.Password);

            return new MyCouchCloudantClient(new CustomCloudantDbClientConnection(uriBuilder.Build()));
        }

        private class CustomCloudantDbClientConnection : DbClientConnection
        {
            public CustomCloudantDbClientConnection(Uri uri)
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
        public ServerClientConfig ServerClient { get; set; }
        public DbClientConfig DbClient { get; set; }
        public bool RunServerClientTests { get; set; }
        public TestEnvironment()
        {
            ServerClient = new ServerClientConfig();
            DbClient = new DbClientConfig();
            RunServerClientTests = false;
        }
    }

    public class ServerClientConfig
    {
        public string Url { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public bool IsAgainstCloudant()
        {
            return Url.Contains("cloudant.com");
        }

        public bool HasCredentials()
        {
            return !string.IsNullOrEmpty(User);
        }
    }

    public class DbClientConfig
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