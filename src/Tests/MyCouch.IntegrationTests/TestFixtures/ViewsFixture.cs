using System;
using System.Linq;
using MyCouch.Commands;
using MyCouch.Querying;
using MyCouch.Testing;
using MyCouch.Testing.Model;

namespace MyCouch.IntegrationTests.TestFixtures
{
    public class ViewsFixture : IDisposable
    {
        private IClient _client;

        public Artist[] Artists { get; protected set; }

        public ViewsFixture()
        {
            Artists = ClientTestData.Artists.CreateArtists(10);

            _client = IntegrationTestsRuntime.CreateClient();

            var bulk = new BulkCommand();
            bulk.Include(Artists.Select(i => _client.Entities.Serializer.Serialize(i)).ToArray());

            var bulkResponse = _client.Documents.BulkAsync(bulk);

            foreach (var row in bulkResponse.Result.Rows)
            {
                var artist = Artists.Single(i => i.ArtistId == row.Id);
                _client.Entities.Reflector.RevMember.SetValueTo(artist, row.Rev);
            }

            _client.Documents.PostAsync(ClientTestData.Views.Artists).Wait();

            var touchView1 = new ViewQuery(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Stale(Stale.UpdateAfter));
            var touchView2 = new ViewQuery(ClientTestData.Views.ArtistsNameNoValueViewId).Configure(q => q.Stale(Stale.UpdateAfter));

            _client.Views.RunQueryAsync(touchView1).Wait();
            _client.Views.RunQueryAsync(touchView2).Wait();
        }

        public virtual void Dispose()
        {
            _client.ClearAllDocuments();
            _client.Dispose();
            _client = null;
        }
    }
}