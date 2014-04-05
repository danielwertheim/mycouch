using System;
using System.Linq;
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

        internal static readonly TestEnvironment Environment;

        static IntegrationTestsRuntime()
        {
            using (var c = new HttpClient())
            {
                Environment = GetTestEnvironment(c, "normal");
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
            var config = environment.ServerClient;
            var uriBuilder = new MyCouchUriBuilder(config.Url);

            if (config.HasCredentials())
                uriBuilder.SetBasicCredentials(config.User, config.Password);

            return config.IsAgainstCloudant()
                ? new MyCouchServerClient(new CustomCloudantServerClientConnection(uriBuilder.Build()))
                : new MyCouchServerClient(uriBuilder.Build());
        }

        internal static IMyCouchClient CreateDbClient(TestEnvironment environment)
        {
            var config = environment.DbClient;
            var uriBuilder = new MyCouchUriBuilder(config.ServerUrl)
                .SetDbName(config.DbName);

            if (config.HasCredentials())
                uriBuilder.SetBasicCredentials(config.User, config.Password);

            return config.IsAgainstCloudant()
                ? new MyCouchClient(new CustomCloudantDbClientConnection(uriBuilder.Build()))
                : new MyCouchClient(uriBuilder.Build());
        }

        internal static IMyCouchCloudantClient CreateCloudantDbClient(TestEnvironment environment)
        {
            var cfg = Environment.DbClient;
            var uriBuilder = new MyCouchUriBuilder(cfg.ServerUrl)
                .SetDbName(cfg.DbName)
                .SetBasicCredentials(cfg.User, cfg.Password);

            return new MyCouchCloudantClient(new CustomCloudantDbClientConnection(uriBuilder.Build()));
        }

        private class CustomCloudantDbClientConnection : DbClientConnection
        {
            public CustomCloudantDbClientConnection(Uri uri)
                : base(uri)
            {
            }

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

        private class CustomCloudantServerClientConnection : ServerClientConnection
        {
            public CustomCloudantServerClientConnection(Uri uri)
                : base(uri)
            {
            }

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

    public static class TestScenarios
    {
        public const string AttachmentsContext = "attachmentscontext";
        public const string ChangesContext = "changescontext";
        public const string DatabaseContext = "databasecontext";
        public const string DatabasesContext = "databasescontext";
        public const string DocumentsContext = "documentscontext";
        public const string EntitiesContext = "entitiescontext";
        public const string ReplicationContext = "replicationcontext";
        public const string ViewsContext = "viewscontext";

        public const string MyCouchStore = "mycouchstore";

        public const string CreateDb = "createdb";
        public const string DeleteDb = "deletedb";
    }

    public class TestEnvironment
    {
        public ServerClientConfig ServerClient { get; set; }
        public DbClientConfig DbClient { get; set; }
        public string TempDbName { get; set; }
        public string[] Supports { get; set; }

        public bool SupportsEverything
        {
            get { return Supports.Contains("*"); }
        }

        public TestEnvironment()
        {
            ServerClient = new ServerClientConfig();
            DbClient = new DbClientConfig();
            Supports = new[] { "*" };
            TempDbName = "mycouchtests-temp";
        }

        public virtual bool HasSupportFor(string requirement)
        {
            return SupportsEverything || Supports.Contains(requirement, StringComparer.OrdinalIgnoreCase);
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