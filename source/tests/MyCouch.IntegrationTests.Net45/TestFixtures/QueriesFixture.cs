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
        internal Blog[] Init(TestEnvironment environment)
        {
            environment.IsAgainstCloudant().Should().Be(true);

            if (_blogs != null && _blogs.Any())
                return _blogs;

            IntegrationTestsRuntime.EnsureCleanEnvironment();

            _blogs = CloudantTestData.Blogs.CreateAll();

            using (var client = IntegrationTestsRuntime.CreateDbClient() as IMyCouchCloudantClient)
            {
                var bulk = new BulkRequest();
                bulk.Include(_blogs.Select(i => client.Entities.Serializer.Serialize(i)).ToArray());

                var bulkResponse = client.Documents.BulkAsync(bulk).Result;

                //foreach (var row in bulkResponse.Rows)
                //{
                //    var blog = _blogs.Single(i => i.Id == row.Id);
                //    client.Entities.Reflector.RevMember.SetValueTo(blog, row.Rev);
                //}

                CreateIndex(client);
            }

            return _blogs;
        }

        private void CreateIndex(IMyCouchCloudantClient client)
        {
            var indexRequest = new PostIndexRequest();
            indexRequest.Configure(q => q.DesignDocument("TestDoc")
                .Name("TestIndex")
                .Fields(new SortableField("title"), new SortableField("author.age"))
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
