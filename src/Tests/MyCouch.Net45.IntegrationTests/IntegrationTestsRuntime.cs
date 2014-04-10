using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using MyCouch.Cloudant;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Responses;
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

            if(Environment.IsAgainstCloudant() && !Environment.HasSupportFor(TestScenarios.Cloudant))
                throw new NotSupportedException("The test environment's ServerClient and/or DbClient is configured to run against Cloudant, but the environment has no support for Cloudant.");
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
                throw new Exception("Could not load test environment settings for: " + TestEnvironmentsBaseUrl + envName, ex);
            }
        }

        internal static IMyCouchServerClient CreateServerClient()
        {
            var config = Environment;
            var uriBuilder = new MyCouchUriBuilder(config.ServerUrl);

            if (config.HasCredentials())
                uriBuilder.SetBasicCredentials(config.User, config.Password);

            return config.IsAgainstCloudant()
                ? new MyCouchServerClient(new CustomCloudantServerClientConnection(uriBuilder.Build()))
                : new MyCouchServerClient(uriBuilder.Build());
        }

        internal static IMyCouchClient CreateDbClient()
        {
            return CreateDbClient(Environment.PrimaryDbName);
        }

        private static IMyCouchClient CreateDbClient(string dbName)
        {
            var config = Environment;
            var uriBuilder = new MyCouchUriBuilder(config.ServerUrl)
                .SetDbName(dbName);

            if (config.HasCredentials())
                uriBuilder.SetBasicCredentials(config.User, config.Password);

            return config.IsAgainstCloudant()
                ? new MyCouchCloudantClient(new CustomCloudantDbClientConnection(uriBuilder.Build()))
                : new MyCouchClient(uriBuilder.Build());
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

        internal static void EnsureCleanEnvironment()
        {
            if (Environment.HasSupportFor(TestScenarios.DeleteDbs))
            {
                DeleteExistingDb(Environment.PrimaryDbName);
                DeleteExistingDb(Environment.SecondaryDbName);
                DeleteExistingDb(Environment.TempDbName);
            }
            else
            {
                ClearAllDocuments(Environment.PrimaryDbName);
                ClearAllDocuments(Environment.SecondaryDbName);
                ClearAllDocuments(Environment.TempDbName);
            }

            if (Environment.HasSupportFor(TestScenarios.CreateDbs))
            {
                CreateDb(Environment.PrimaryDbName);
                CreateDb(Environment.SecondaryDbName);
                CreateDb(Environment.TempDbName);
            }

        }

        private static void CreateDb(string dbName)
        {
            using (var client = CreateServerClient())
            {
                var put = client.Databases.PutAsync(dbName).Result;
                if (!put.IsSuccess)
                    throw new MyCouchException(put);
            }
        }

        private static void DeleteExistingDb(string dbName)
        {
            using (var client = CreateServerClient())
            {
                if (client.Databases.HeadAsync(dbName).Result.StatusCode == HttpStatusCode.NotFound)
                    return;

                var delete = client.Databases.DeleteAsync(dbName).Result;
                if (!delete.IsSuccess)
                    throw new MyCouchException(delete);
            }
        }

        private static void ClearAllDocuments(string dbName)
        {
            using (var client = CreateDbClient(dbName))
            {
                if (client.Database.HeadAsync().Result.StatusCode == HttpStatusCode.NotFound)
                    return;

                var query = new QueryViewRequest("_all_docs").Configure(q => q.Stale(Stale.UpdateAfter));
                var response = client.Views.QueryAsync<dynamic>(query).Result;

                BulkDelete(client, response);

                response = client.Views.QueryAsync<dynamic>(query).Result;

                BulkDelete(client, response);
            }
        }

        private static void BulkDelete(IMyCouchClient client, ViewQueryResponse<dynamic> response)
        {
            if (response.IsEmpty)
                return;

            var bulkRequest = new BulkRequest();

            foreach (var row in response.Rows)
                bulkRequest.Delete(row.Id, row.Value.rev.ToString());

            client.Documents.BulkAsync(bulkRequest).Wait();
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
        public const string ViewsContext = "viewscontext";
        public const string SearchesContext = "searchescontext";

        public const string Cloudant = "cloudant";
        public const string MyCouchStore = "mycouchstore";

        public const string CreateDbs = "createdbs";
        public const string DeleteDbs = "deletedbs";
        public const string CompactDbs = "compactdbs";
        public const string Replication = "replication";
    }

    public class TestEnvironment
    {
        public string[] Supports { get; set; }
        public string ServerUrl { get; set; }
        public string PrimaryDbName { get; set; }
        public string SecondaryDbName { get; set; }
        public string TempDbName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        
        public bool HasCredentials()
        {
            return !string.IsNullOrEmpty(User);
        }

        public bool IsAgainstCloudant()
        {
            return ServerUrl.Contains("cloudant.com");
        }

        public bool SupportsEverything
        {
            get { return Supports.Contains("*"); }
        }

        public TestEnvironment()
        {
            Supports = new[] { "*" };
            ServerUrl = "http://localhost:5984";
            PrimaryDbName = "mycouchtests_pri";
            SecondaryDbName = "mycouchtests_sec";
            TempDbName = PrimaryDbName + "_tmp";
            User = "sa";
            Password = "p@ssword";
        }

        public virtual bool HasSupportFor(params string[] requirements)
        {
            return SupportsEverything || requirements.All(r => Supports.Contains(r, StringComparer.OrdinalIgnoreCase));
        }
    }
}