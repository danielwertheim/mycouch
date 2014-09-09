using System;
using System.Collections.Generic;
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
        internal static readonly TestEnvironment Environment;

        static IntegrationTestsRuntime()
        {
            Environment = TestEnvironments.GetMachineSpecificOrDefaultTestEnvironment();

            if(Environment.IsAgainstCloudant() && !Environment.HasSupportFor(TestScenarios.Cloudant))
                throw new NotSupportedException("The test environment's ServerClient and/or DbClient is configured to run against Cloudant, but the environment has no support for Cloudant.");
        }

        internal static IMyCouchServerClient CreateServerClient()
        {
            var config = Environment;
            var uriBuilder = new MyCouchUriBuilder(config.ServerUrl);

            if (config.HasCredentials())
                uriBuilder.SetBasicCredentials(config.User, config.Password);

            return config.IsAgainstCloudant()
                ? (IMyCouchServerClient)new MyCouchCloudantServerClient(new CustomCloudantServerClientConnection(uriBuilder.Build()))
                : (IMyCouchServerClient)new MyCouchServerClient(uriBuilder.Build());
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

            protected override HttpRequestMessage CreateHttpRequestMessage(HttpRequest httpRequest)
            {
                var message = base.CreateHttpRequestMessage(httpRequest);

                if (message.Method == HttpMethod.Post || message.Method == HttpMethod.Put || message.Method == HttpMethod.Delete)
                {
                    message.RequestUri = string.IsNullOrEmpty(message.RequestUri.Query)
                        ? new Uri(message.RequestUri + "?w=3")
                        : new Uri(message.RequestUri + "&w=3");
                }

                if (message.Method == HttpMethod.Get || message.Method == HttpMethod.Head)
                {
                    message.RequestUri = string.IsNullOrEmpty(message.RequestUri.Query)
                        ? new Uri(message.RequestUri + "?r=3")
                        : new Uri(message.RequestUri + "&r=3");
                }

                return message;
            }
        }

        private class CustomCloudantServerClientConnection : ServerClientConnection
        {
            public CustomCloudantServerClientConnection(Uri uri)
                : base(uri)
            {
            }

            protected override HttpRequestMessage CreateHttpRequestMessage(HttpRequest httpRequest)
            {
                var message = base.CreateHttpRequestMessage(httpRequest);

                if (message.Method == HttpMethod.Post || message.Method == HttpMethod.Put || message.Method == HttpMethod.Delete)
                {
                    message.RequestUri = string.IsNullOrEmpty(message.RequestUri.Query)
                        ? new Uri(message.RequestUri + "?w=3")
                        : new Uri(message.RequestUri + "&w=3");
                }

                if (message.Method == HttpMethod.Get || message.Method == HttpMethod.Head)
                {
                    message.RequestUri = string.IsNullOrEmpty(message.RequestUri.Query)
                        ? new Uri(message.RequestUri + "?r=1")
                        : new Uri(message.RequestUri + "&r=1");
                }

                return message;
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

            if(Environment.HasSupportFor(TestScenarios.Replication))
                ClearAllDocuments("_replicator");
        }

        private static void CreateDb(string dbName)
        {
            using (var client = CreateServerClient())
            {
                var put = client.Databases.PutAsync(dbName).Result;
                if (!put.IsSuccess)
                    throw new MyCouchResponseException(put);
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
                    throw new MyCouchResponseException(delete);
            }
        }

        private static void ClearAllDocuments(string dbName)
        {
            using (var client = CreateDbClient(dbName))
            {
                if (client.Database.HeadAsync().Result.StatusCode == HttpStatusCode.NotFound)
                    return;

                var query = new QueryViewRequest(SystemViewIdentity.AllDocs).Configure(q => q.Stale(Stale.UpdateAfter));
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
            {
                if (row.Id.ToLower() == "_design/_replicator")
                    continue;

                bulkRequest.Delete(row.Id, row.Value.rev.ToString());
            }

            if(!bulkRequest.IsEmpty)
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
        public const string SecurityContext = "securitycontext";
        public const string SearchesContext = "searchescontext";
        public const string QueriesContext = "queriescontext";

        public const string Cloudant = "cloudant";
        public const string MyCouchStore = "mycouchstore";

        public const string CreateDbs = "createdbs";
        public const string DeleteDbs = "deletedbs";
        public const string CompactDbs = "compactdbs";
        public const string ViewCleanUp = "compactdbs";
        public const string Replication = "replication";
    }

    public class TestEnvironment
    {
        public const string DefaultEnvironmentKey = "Default";

        public string Key { get; set; }
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
            return ServerUrl.ToLower().Contains("cloudant.com");
        }

        public bool SupportsEverything
        {
            get { return Supports.Contains("*"); }
        }

        public TestEnvironment()
        {
            Key = DefaultEnvironmentKey;
            Supports = new[] { "*" };
            ServerUrl = "http://localhost:5984";
            PrimaryDbName = "mycouchtests_pri";
            SecondaryDbName = "mycouchtests_sec";
            TempDbName = "mycouchtests_tmp";
            User = "sa";
            Password = "test";
        }

        public virtual bool HasSupportFor(params string[] requirements)
        {
            return SupportsEverything || requirements.All(r => Supports.Contains(r, StringComparer.OrdinalIgnoreCase));
        }
    }

    public static class TestEnvironments
    {
        private const string TestEnvironmentsBaseUrl = "http://localhost:8991/testenvironments/";

        public static TestEnvironment GetMachineSpecificOrDefaultTestEnvironment()
        {
            var environments = GetTestEnvironments();
            TestEnvironment machineSpecific;

            return environments.TryGetValue(Environment.MachineName, out machineSpecific)
                ? machineSpecific
                : environments[TestEnvironment.DefaultEnvironmentKey];
        }

        private static IDictionary<string, TestEnvironment> GetTestEnvironments()
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage r;
                const string exMessage = "Could not load test environments from: " + TestEnvironmentsBaseUrl;

                try
                {
                    r = client.GetAsync(TestEnvironmentsBaseUrl).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception(exMessage, ex);
                }

                if (!r.IsSuccessStatusCode)
                    throw new Exception(exMessage);

                var environments = JsonConvert.DeserializeObject<TestEnvironment[]>(r.Content.ReadAsStringAsync().Result);
                if (environments == null || !environments.Any())
                    throw new Exception(exMessage);

                return environments.ToDictionary(e => e.Key, e => e);
            }
        }
    }
}