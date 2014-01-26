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

            using (var client = IntegrationTestsRuntime.CreateClient(_environment))
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

                var touchView1 = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Stale(Stale.UpdateAfter));
                var touchView2 = new QueryViewRequest(ClientTestData.Views.ArtistsNameNoValueViewId).Configure(q => q.Stale(Stale.UpdateAfter));
                var touchView3 = new QueryViewRequest(ClientTestData.Views.ArtistsTotalNumOfAlbumsViewId).Configure(q => q.Stale(Stale.UpdateAfter));

                Task.WaitAll(
                    client.Views.QueryAsync(touchView1),
                    client.Views.QueryAsync(touchView2),
                    client.Views.QueryAsync(touchView3));
            }
        }

        public virtual void Dispose()
        {
            if(_environment == null)
                return;

            using (var client = IntegrationTestsRuntime.CreateClient(_environment))
            {
                client.ClearAllDocuments();
            }
        }
    }
}