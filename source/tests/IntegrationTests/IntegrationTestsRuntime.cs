using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using MyCouch;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Responses;
using Newtonsoft.Json;

namespace IntegrationTests
{
    internal static class IntegrationTestsRuntime
    {
        internal static readonly TestEnvironment Environment;
        internal static readonly Config Config;

        static IntegrationTestsRuntime()
        {
            Environment = TestEnvironment.Read();
            Config = Config.Read();
        }

        internal static IMyCouchServerClient CreateServerClient()
        {
            var config = Environment;
            var connectionInfo = new ServerConnectionInfo(config.ServerUrl)
            {
                BasicAuth = new BasicAuthString(Config.GetUsername(), Config.GetPassword())
            };

            return new MyCouchServerClient(connectionInfo, new MyCouchClientBootstrapper
            {
                ServerConnectionFn = cnInfo => new CustomServerConnection(cnInfo)
            });
        }

        internal static IMyCouchClient CreateDbClient()
        {
            return CreateDbClient(Environment.PrimaryDbName);
        }

        private static IMyCouchClient CreateDbClient(string dbName)
        {
            var config = Environment;
            var connectionInfo = new DbConnectionInfo(config.ServerUrl, dbName)
            {
                BasicAuth = new BasicAuthString(Config.GetUsername(), Config.GetPassword())
            };

            return new MyCouchClient(connectionInfo, new MyCouchClientBootstrapper
            {
                DbConnectionFn = cnInfo => new CustomDbConnection(cnInfo)
            });
        }

        private class CustomDbConnection : DbConnection
        {
            public CustomDbConnection(DbConnectionInfo connectionInfo) : base(connectionInfo) { }

            protected override HttpRequestMessage CreateHttpRequestMessage(HttpRequest httpRequest)
            {
                var message = base.CreateHttpRequestMessage(httpRequest);

                if (message.Method == HttpMethod.Post || message.Method == HttpMethod.Put || message.Method == HttpMethod.Delete)
                {
                    message.RequestUri = string.IsNullOrEmpty(message.RequestUri.Query)
                        ? new Uri(message.RequestUri + "?w=1")
                        : new Uri(message.RequestUri + "&w=1");
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

        private class CustomServerConnection : ServerConnection
        {
            public CustomServerConnection(ServerConnectionInfo connectionInfo) : base(connectionInfo) { }

            protected override HttpRequestMessage CreateHttpRequestMessage(HttpRequest httpRequest)
            {
                var message = base.CreateHttpRequestMessage(httpRequest);

                if (message.Method == HttpMethod.Post || message.Method == HttpMethod.Put || message.Method == HttpMethod.Delete)
                {
                    message.RequestUri = string.IsNullOrEmpty(message.RequestUri.Query)
                        ? new Uri(message.RequestUri + "?w=1")
                        : new Uri(message.RequestUri + "&w=1");
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

            if (Environment.HasSupportFor(TestScenarios.Replication))
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

            if (!bulkRequest.IsEmpty)
                client.Documents.BulkAsync(bulkRequest).Wait();
        }
    }

    public static class TestScenarios
    {
        public const string Client = "client";
        public const string AttachmentsContext = "attachmentscontext";
        public const string ChangesContext = "changescontext";
        public const string DatabaseContext = "databasecontext";
        public const string DatabasesContext = "databasescontext";
        public const string DocumentsContext = "documentscontext";
        public const string EntitiesContext = "entitiescontext";
        public const string ViewsContext = "viewscontext";
        //public const string SearchesContext = "searchescontext";
        public const string QueriesContext = "queriescontext";
        public const string ListsContext = "listscontext";
        public const string ShowsContext = "showscontext";

        public const string MyCouchStore = "mycouchstore";

        public const string CreateDbs = "createdbs";
        public const string DeleteDbs = "deletedbs";
        public const string CompactDbs = "compactdbs";
        public const string ViewCleanUp = "compactdbs";
        public const string Replication = "replication";
    }

    public class TestEnvironment
    {
        public string[] SupportedScenarios { get; set; }
        public string ServerUrl { get; set; }
        public string PrimaryDbName { get; set; }
        public string SecondaryDbName { get; set; }
        public string TempDbName { get; set; }

        public bool SupportsEverything
            => SupportedScenarios.Contains("*");

        public virtual bool HasSupportFor(params string[] requirements)
            => SupportsEverything || requirements.All(r => SupportedScenarios.Contains(r, StringComparer.OrdinalIgnoreCase));

        public static TestEnvironment Read()
        {
            var fullPath = GetTestEnvironmentFullPath(@".\integrationtests.json");
            var content = File.ReadAllText(fullPath);
            var environment = JsonConvert.DeserializeObject<TestEnvironment>(content);
            if (environment == null)
                throw new Exception("Could not load TestEnvironments for integration tests from file: " + fullPath);

            return environment;
        }

        private static string GetTestEnvironmentFullPath(string relativePath)
        {
            var fullPath = Path.GetFullPath(relativePath);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Could not find test environments JSON file.", relativePath);

            return fullPath;
        }
    }

    public class Config
    {
        private readonly IConfigurationRoot _configRoot;

        private Config(IConfigurationRoot configRoot)
        {
            _configRoot = configRoot;
        }

        public static Config Read()
        {
            var builder = new ConfigurationBuilder();
            var configRoot = builder
                .AddIniFile("integrationtests.local.ini", true, false)
                .AddEnvironmentVariables("MYCOUCH_")
                .Build();

            return new Config(configRoot);
        }

        private string GetRequiredValue(string path)
        {
            var value = _configRoot[path];
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception($"Can not read '{path}' from INI-file or environment variables.");

            return value;
        }

        public string GetUsername() => GetRequiredValue("Credentials:User");

        public string GetPassword() => GetRequiredValue("Credentials:Pass");
    }
}