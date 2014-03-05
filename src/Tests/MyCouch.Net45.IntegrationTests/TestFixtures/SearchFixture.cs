using System;
using System.Linq;
using MyCouch.Requests;
using MyCouch.Testing.Model;
using MyCouch.Testing.TestData;

namespace MyCouch.IntegrationTests.TestFixtures
{
    public class SearchFixture : IDisposable
    {
        private TestEnvironment _environment;

        public Animal[] Animals { get; protected set; }

        public void Init(TestEnvironment environment)
        {
            if (_environment != null)
                return;

            _environment = environment;

            Animals = CloudantTestData.Animals.CreateAll();

            using (var client = IntegrationTestsRuntime.CreateClient(_environment))
            {
                client.ClearAllDocuments();

                var bulk = new BulkRequest();
                bulk.Include(Animals.Select(i => client.Entities.Serializer.Serialize(i)).ToArray());

                var bulkResponse = client.Documents.BulkAsync(bulk).Result;

                foreach (var row in bulkResponse.Rows)
                {
                    var animal = Animals.Single(i => i.AnimalId == row.Id);
                    client.Entities.Reflector.RevMember.SetValueTo(animal, row.Rev);
                }

                client.Documents.PostAsync(CloudantTestData.Views.Views101).Wait();

                var queries = CloudantTestData.Views.AllViewIds.Select(id => new QueryViewRequest(id).Configure(q => q.Stale(Stale.UpdateAfter)));
                foreach (var query in queries)
                    client.Views.QueryAsync(query).Wait();
            }
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

            if (_environment == null)
                return;

            using (var client = IntegrationTestsRuntime.CreateClient(_environment))
            {
                client.ClearAllDocuments();
            }
        }
    }
}