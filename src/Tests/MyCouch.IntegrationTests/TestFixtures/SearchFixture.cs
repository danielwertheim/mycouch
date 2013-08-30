using System;
using System.Linq;
using MyCouch.Cloudant;
using MyCouch.Commands;
using MyCouch.Querying;
using MyCouch.Testing;
using MyCouch.Testing.Model;

namespace MyCouch.IntegrationTests.TestFixtures
{
    public class SearchFixture : IDisposable
    {
        private ICloudantClient _client;

        public Animal[] Animals { get; protected set; }

        public SearchFixture()
        {
            Animals = CloudantTestData.Animals.CreateAll();

            _client = IntegrationTestsRuntime.CreateCloudantClient();

            var bulk = new BulkCommand();
            bulk.Include(Animals.Select(i => _client.Entities.Serializer.Serialize(i)).ToArray());

            var bulkResponse = _client.Documents.BulkAsync(bulk).Result;

            foreach (var row in bulkResponse.Rows)
            {
                var animal = Animals.Single(i => i.AnimalId == row.Id);
                _client.Entities.Reflector.RevMember.SetValueTo(animal, row.Rev);
            }

            _client.Documents.PostAsync(CloudantTestData.Views.Views101).Wait();

            var queries = CloudantTestData.Views.AllViewIds.Select(id => new ViewQuery(id).Configure(q => q.Stale(Stale.UpdateAfter)));
            foreach (var query in queries)
                _client.Views.RunQueryAsync(query).Wait();
        }

        public virtual void Dispose()
        {
            _client.ClearAllDocuments();
            _client.Dispose();
            _client = null;
        }
    }
}