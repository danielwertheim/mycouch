using System;
using System.Linq;
using MyCouch.Requests;
using MyCouch.Testing.Model;
using MyCouch.Testing.TestData;

namespace IntegrationTests.TestFixtures
{
    public class ShowsFixture : IDisposable
    {
        private Artist[] _artists;

        public Artist[] Init(TestEnvironment environment)
        {
            if(_artists != null && _artists.Any())
                return _artists;

            IntegrationTestsRuntime.EnsureCleanEnvironment();

            _artists = ClientTestData.Artists.CreateArtists(2);

            using (var client = IntegrationTestsRuntime.CreateDbClient())
            {
                var bulk = new BulkRequest();
                bulk.Include(_artists.Select(i => client.Entities.Serializer.Serialize(i)).ToArray());

                var bulkResponse = client.Documents.BulkAsync(bulk).Result;

                foreach (var row in bulkResponse.Rows)
                {
                    var artist = _artists.Single(i => i.ArtistId == row.Id);
                    client.Entities.Reflector.RevMember.SetValueTo(artist, row.Rev);
                }

                client.Documents.PostAsync(ClientTestData.Shows.ArtistsShows).Wait();
            }

            return _artists;
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

            IntegrationTestsRuntime.EnsureCleanEnvironment();
        }
    }
}