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
        private static readonly TestEnvironment NormalClientEnvironment;
        private static readonly TestEnvironment CloudantClientEnvironment;

        static IntegrationTestsRuntime()
        {
            using (var c = new HttpClient())
            {
                NormalClientEnvironment = GetTestEnvironment(c, "normal");
                CloudantClientEnvironment = GetTestEnvironment(c, "cloudant");
            }

            if (NormalClientEnvironment != null)
            {
                using (var client = CreateNormalClient())
                {
                    //client.Database.PutAsync().Wait();
                    client.ClearAllDocuments();
                }
            }

            if (CloudantClientEnvironment != null)
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

        internal static IMyCouchClient CreateNormalClient()
        {
            if(NormalClientEnvironment == null)
                throw new Exception("Can not create Normal client. Missing configuration.");

            var cfg = NormalClientEnvironment.Client;
            var uriBuilder = new MyCouchUriBuilder(cfg.ServerUrl)
                .SetDbName(cfg.DbName)
                .SetBasicCredentials(cfg.User, cfg.Password);

            return cfg.IsAgainstCloudant
                ? new MyCouchClient(new CustomCloudantConnection(uriBuilder.Build()))
                : new MyCouchClient(uriBuilder.Build());
        }

        internal static IMyCouchCloudantClient CreateCloudantClient()
        {
            if (CloudantClientEnvironment == null)
                throw new Exception("Can not create Cloudant client. Missing configuration.");

            var cfg = CloudantClientEnvironment.Client;
            var uriBuilder = new MyCouchUriBuilder(cfg.ServerUrl)
                .SetDbName(cfg.DbName)
                .SetBasicCredentials(cfg.User, cfg.Password);

            return new MyCouchCloudantClient(new CustomCloudantConnection(uriBuilder.Build()));
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
            public bool IsAgainstCloudant { get { return ServerUrl.Contains("cloudant.com"); } }
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
}