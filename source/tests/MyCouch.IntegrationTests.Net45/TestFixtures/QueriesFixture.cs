using MyCouch.Cloudant;
using MyCouch.Cloudant.Requests;
using MyCouch.Requests;
using MyCouch.Testing.Model;
using MyCouch.Testing.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace MyCouch.IntegrationTests.TestFixtures
{
    public class QueriesFixture : IDisposable
    {
        private Blog[] _blogs;
        internal void Init(TestEnvironment environment)
        {
            environment.IsAgainstCloudant().Should().Be(true);

            if (_blogs != null && _blogs.Any())
                return;

            IntegrationTestsRuntime.EnsureCleanEnvironment();

            _blogs = CloudantTestData.Blogs.CreateAll();

            using (var client = IntegrationTestsRuntime.CreateDbClient() as IMyCouchCloudantClient)
            {
                var bulk = new BulkRequest();
                bulk.Include(_blogs.Select(i => client.Entities.Serializer.Serialize(i)).ToArray());

                var bulkResponse = client.Documents.BulkAsync(bulk).Result;

                CreateIndexes(client);
            }
        }

        private void CreateIndexes(IMyCouchCloudantClient client)
        {
            CreateIndex1(client);
            CreateIndex2(client);
        }

        private static void CreateIndex1(IMyCouchCloudantClient client)
        {
            var indexRequest = new PostIndexRequest();
            indexRequest.Configure(q => q.DesignDocument("TestDoc")
                .Name("TestIndex1")
                .Fields(new SortableField("title"), new SortableField("author.age"), new SortableField("yearsActive"))
                );

            var response = client.Queries.PostAsync(indexRequest).Result;

            response.IsSuccess.Should().Be(true);
        }

        private static void CreateIndex2(IMyCouchCloudantClient client)
        {
            var indexRequest = new PostIndexRequest();
            indexRequest.Configure(q => q.DesignDocument("TestDoc")
                .Name("TestIndex2")
                .Fields(new SortableField("author.age"), new SortableField("yearsActive"))
                );

            var response = client.Queries.PostAsync(indexRequest).Result;

            response.IsSuccess.Should().Be(true);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            IntegrationTestsRuntime.EnsureCleanEnvironment();
        }
    }
}
