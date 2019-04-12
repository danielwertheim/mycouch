using System;
using System.Linq;
using System.Threading.Tasks;
using MyCouch;
using MyCouch.Requests;
using MyCouch.Testing.Model;
using MyCouch.Testing.TestData;

namespace IntegrationTests.TestFixtures
{
    public class SearchFixture : IDisposable
    {
        private Animal[] _animals;

        public Animal[] Init(TestEnvironment environment)
        {
            if (_animals != null && _animals.Any())
                return _animals;

            IntegrationTestsRuntime.EnsureCleanEnvironment();

            _animals = CloudantTestData.Animals.CreateAll();

            using (var client = IntegrationTestsRuntime.CreateDbClient())
            {
                var bulk = new BulkRequest();
                bulk.Include(_animals.Select(i => client.Entities.Serializer.Serialize(i)).ToArray());

                var bulkResponse = client.Documents.BulkAsync(bulk).Result;

                foreach (var row in bulkResponse.Rows)
                {
                    var animal = _animals.Single(i => i.AnimalId == row.Id);
                    client.Entities.Reflector.RevMember.SetValueTo(animal, row.Rev);
                }

                client.Documents.PostAsync(CloudantTestData.Views.Views101).Wait();

                var queryRequests = CloudantTestData.Views.AllViewIds.Select(id => new QueryViewRequest(id).Configure(q => q.Stale(Stale.UpdateAfter)));
                var queries = queryRequests.Select(q => client.Views.QueryAsync(q) as Task).ToArray();
                Task.WaitAll(queries);
            }

            return _animals;
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