using System;
using System.Linq;
using MyCouch.Cloudant;
using MyCouch.Requests;
using MyCouch.Testing.Model;
using MyCouch.Testing.TestData;

namespace MyCouch.IntegrationTests.TestFixtures
{
    public class SearchFixture : IDisposable
    {
        protected IMyCouchCloudantClient Client;

        public Animal[] Animals { get; protected set; }

        public SearchFixture()
        {
            Animals = CloudantTestData.Animals.CreateAll();

            Client = IntegrationTestsRuntime.CreateCloudantClient();

            var bulk = new BulkRequest();
            bulk.Include(Animals.Select(i => Client.Entities.Serializer.Serialize(i)).ToArray());

            var bulkResponse = Client.Documents.BulkAsync(bulk).Result;

            foreach (var row in bulkResponse.Rows)
            {
                var animal = Animals.Single(i => i.AnimalId == row.Id);
                Client.Entities.Reflector.RevMember.SetValueTo(animal, row.Rev);
            }

            Client.Documents.PostAsync(CloudantTestData.Views.Views101).Wait();

            var queries = CloudantTestData.Views.AllViewIds.Select(id => new QueryViewRequest(id).Configure(q => q.Stale(Stale.UpdateAfter)));
            foreach (var query in queries)
                Client.Views.QueryAsync(query).Wait();
        }

        public virtual void Dispose()
        {
            Client.ClearAllDocuments();
            Client.Dispose();
            Client = null;
        }
    }
}