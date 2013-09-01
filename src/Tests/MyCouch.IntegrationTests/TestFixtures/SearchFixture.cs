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
        protected ICloudantClient Client;

        public Animal[] Animals { get; protected set; }

        public SearchFixture()
        {
            Animals = CloudantTestData.Animals.CreateAll();

            Client = IntegrationTestsRuntime.CreateCloudantClient();

            var bulk = new BulkCommand();
            bulk.Include(Animals.Select(i => Client.Entities.Serializer.Serialize(i)).ToArray());

            var bulkResponse = Client.Documents.BulkAsync(bulk).Result;

            foreach (var row in bulkResponse.Rows)
            {
                var animal = Animals.Single(i => i.AnimalId == row.Id);
                Client.Entities.Reflector.RevMember.SetValueTo(animal, row.Rev);
            }

            Client.Documents.PostAsync(CloudantTestData.Views.Views101).Wait();

            var queries = CloudantTestData.Views.AllViewIds.Select(id => new ViewQuery(id).Configure(q => q.Stale(Stale.UpdateAfter)));
            foreach (var query in queries)
                Client.Views.RunQueryAsync(query).Wait();
        }

        public virtual void Dispose()
        {
            Client.ClearAllDocuments();
            Client.Dispose();
            Client = null;
        }
    }
}