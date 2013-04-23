using System.Collections.Generic;
using System.Linq;
using MyCouch.Testing.Model;

namespace MyCouch.Testing
{
    public static class TestDataFactory
    {
        public static Artist CreateArtist()
        {
            return CreateArtists(1).Single();
        }

        public static Artist[] CreateArtists(int numOf)
        {
            var artists = new List<Artist>();
            var numOfAlbums = new[] { 1, 2, 3 };

            for (var c = 0; c < numOf; c++)
            {
                var artist = new Artist
                {
                    ArtistId = c.ToString(),
                    Name = "Artist #" + numOf
                };

                artist.Albums = CreateAlbums(numOfAlbums[c % numOfAlbums.Length], artist.Name);

                artists.Add(artist);
            }

            return artists.ToArray();
        }

        private static Album[] CreateAlbums(int numOf, string artistName)
        {
            var albums = new List<Album>();

            for (var c = 0; c < numOf; c++)
            {
                albums.Add(new Album
                {
                    Name = string.Format("{0} - Greatest fakes #{1}", artistName, c + 1)
                });
            }

            return albums.ToArray();
        }
    }
}