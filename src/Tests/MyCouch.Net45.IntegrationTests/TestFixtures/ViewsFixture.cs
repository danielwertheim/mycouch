using System;
using System.Linq;
using System.Threading.Tasks;
using MyCouch.Requests;
using MyCouch.Testing.Model;
using MyCouch.Testing.TestData;

namespace MyCouch.IntegrationTests.TestFixtures
{
    public class ViewsFixture : IDisposable
    {
        private TestEnvironment _environment;

        public Artist[] Artists { get; private set; }

        public void Init(TestEnvironment environment)
        {
            if(_environment != null)
                return;

            _environment = environment;

            Artists = ClientTestData.Artists.CreateArtists(10);

            using (var client = IntegrationTestsRuntime.CreateDbClient(_environment))
            {
                client.ClearAllDocuments();

                var bulk = new BulkRequest();
                bulk.Include(Artists.Select(i => client.Entities.Serializer.Serialize(i)).ToArray());

                var bulkResponse = client.Documents.BulkAsync(bulk).Result;

                foreach (var row in bulkResponse.Rows)
                {
                    var artist = Artists.Single(i => i.ArtistId == row.Id);
                    client.Entities.Reflector.RevMember.SetValueTo(artist, row.Rev);
                }

                var tmp = client.Documents.PostAsync(ClientTestData.Views.ArtistsViews).Result;

                var queryRequests = ClientTestData.Views.AllViewIds.Select(id => new QueryViewRequest(id).Configure(q => q.Stale(Stale.UpdateAfter)));
                var queries = queryRequests.Select(q => client.Views.QueryAsync(q) as Task).ToArray();
                Task.WaitAll(queries);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!disposing)
                return;

            if(_environment == null)
                return;

            using (var client = IntegrationTestsRuntime.CreateDbClient(_environment))
            {
                client.ClearAllDocuments();
            }
        }
    }
}