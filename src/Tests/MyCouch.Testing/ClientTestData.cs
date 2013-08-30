using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyCouch.Net;
using MyCouch.Testing.Model;

namespace MyCouch.Testing
{
    public static class ClientTestData
    {
        public static class Artists
        {
            public const string Artist1Id = "test:1";
            public const string Artist2Id = "test:2";
            public const string Artist3Id = "test:3";
            public const string Artist4Id = "test:4";

            public static readonly Artist Artist1 = new Artist
            {
                ArtistId = Artist1Id,
                Name = "Fake artist 1", 
                Albums = new[]
                {
                    new Album { Name = "Greatest fakes #1" }
                }
            };
            public static readonly Artist Artist2 = new Artist
            {
                ArtistId = Artist2Id, 
                Name = "Fake artist 1", 
                Albums = new[]
                {
                    new Album { Name = "Greatest fakes #2.1" }, new Album { Name = "Greatest fakes #2.2" }
                }
            };
            public static readonly Artist Artist3 = new Artist
            {
                ArtistId = Artist3Id, 
                Name = "Fake artist 1", 
                Albums = new[]
                {
                    new Album { Name = "Greatest fakes #3.1" }, new Album { Name = "Greatest fakes #3.2" }, new Album { Name = "Greatest fakes #3.3" }
                }
            };
            public static readonly Artist Artist4 = new Artist
            {
                ArtistId = Artist4Id,
                Name = "Fake artist 1", 
                Albums = new[]
                {
                    new Album { Name = "Greatest fakes #4.1" }, new Album { Name = "Greatest fakes #4.2" }, new Album { Name = "Greatest fakes #4.3" }, new Album { Name = "Greatest fakes #4.4" }
                }
            };

            public const string Artist1Json = "{\"_id\": \"test:1\", \"$doctype\": \"artist\", \"name\": \"Fake artist 1\", \"albums\":[{\"name\": \"Greatest fakes #1\"}]}";
            public const string Artist2Json = "{\"_id\": \"test:2\", \"$doctype\": \"artist\", \"name\": \"Fake artist 2\", \"albums\":[{\"name\": \"Greatest fakes #2.1\"},{\"name\": \"Greatest fakes #2.2\"}]}";
            public const string Artist3Json = "{\"_id\": \"test:3\", \"$doctype\": \"artist\", \"name\": \"Fake artist 3\", \"albums\":[{\"name\": \"Greatest fakes #3.1\"},{\"name\": \"Greatest fakes #3.2\"},{\"name\": \"Greatest fakes #3.3\"}]}";
            public const string Artist4Json = "{\"_id\": \"test:4\", \"$doctype\": \"artist\", \"name\": \"Fake artist 4\", \"albums\":[{\"name\": \"Greatest fakes #4.1\"},{\"name\": \"Greatest fakes #4.2\"},{\"name\": \"Greatest fakes #4.3\"},{\"name\": \"Greatest fakes #4.4\"}]}";

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
                        ArtistId = string.Format("test:{0}", (c + 1)),
                        Name = "Fake artist " + (c + 1)
                    };

                    artist.Albums = CreateAlbums(numOfAlbums[c % numOfAlbums.Length], c);

                    artists.Add(artist);
                }

                return artists.OrderBy(a => a.Name).ToArray();
            }

            private static Album[] CreateAlbums(int numOf, int artistIndex)
            {
                var artistNum = artistIndex + 1;
                var albums = new List<Album>();

                for (var c = 0; c < numOf; c++)
                {
                    albums.Add(new Album
                    {
                        Name = string.Format("Greatest fakes #{0}.{1}", artistNum + 1, c + 1)
                    });
                }

                return albums.ToArray();
            }
        }

        public static class Views
        {
            public const string ArtistsViews =
                "{" +
                    "\"_id\": \"_design/artists\"," +
                    "\"language\": \"javascript\"," +
                    "\"views\": {" +
                        "\"albums\": {" +
                            "\"map\": \"function(doc) {  if(doc.$doctype !== 'artist') return;  emit(doc.name, doc.albums);}\"" +
                        "}," +
                        "\"name_no_value\": {" +
                            "\"map\": \"function(doc) {  if(doc.$doctype !== 'artist') return;  emit(doc.name, null);}\"" +
                        "}" +
                    "}" +
                "}";
            public static readonly IViewIdentity ArtistsAlbumsViewId = new ViewIdentity("artists", "albums");
            public static readonly IViewIdentity ArtistsNameNoValueViewId = new ViewIdentity("artists", "name_no_value");
        }

        public static class Attachments
        {
            public static class One
            {
                public const string Name = "att:1";
                public const string ContentEncoded = "TXlDb3VjaCwgdGhlIHNpbXBsZSBhc3luY2hyb25vdXMgY2xpZW50IGZvciAuTmV0";
                public const string ContentDecoded = "MyCouch, the simple asynchronous client for .Net";
                public static readonly string ContentType = HttpContentTypes.Text;
            }
        }

        public static string AsBase64EncodedString(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static byte[] AsBytes(this string content)
        {
            return MyCouchRuntime.DefaultEncoding.GetBytes(content);
        }

        public static Stream AsStream(this string content)
        {
            return new MemoryStream(MyCouchRuntime.DefaultEncoding.GetBytes(content));
        }
    }
}