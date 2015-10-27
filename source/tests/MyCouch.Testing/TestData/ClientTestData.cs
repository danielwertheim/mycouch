using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyCouch.Net;
using MyCouch.Testing.Model;

namespace MyCouch.Testing.TestData
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

                return artists.OrderBy(a => a.ArtistId).ToArray();
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

                return albums.OrderBy(a => a.Name).ToArray();
            }
        }

        public static class Shows
        {
            public const string ArtistsShows =
                "{" +
                    "\"_id\": \"_design/artistshows\"," +
                    "\"shows\": {" +
                        "\"hello\": \"function(doc, req){" +
                                    "return '<h1>hello</h1>';" +
                                "}\"," +
                        "\"jsonShow\": \"function(doc, req){" +
                                    "provides('json',function(){" +
                                        "send(JSON.stringify({ name: doc.name}));" +
                                    "});" +
                                "}\"," +
                        "\"xmlShow\": \"function(doc, req){" +
                                    "provides('xml',function(){" +
                                        "html = '<foo>' + doc.name + '</foo>';" +
                                        "return html;" +
                                    "});" +
                                "}\"," +
                        "\"jsonCustomQueryParamShow\": \"function(doc, req){" +
                                    "provides('json',function(){" +
                                        "send(JSON.stringify({ name: doc.name, foo: req.query.foo}));" +
                                    "});" +
                                "}\"" +
                    "}" +
                "}";
            public static readonly ShowIdentity ArtistsHelloShowId = new ShowIdentity("artistshows", "hello");
            public static readonly ShowIdentity ArtistsJsonShowId = new ShowIdentity("artistshows", "jsonShow");
            public static readonly ShowIdentity ArtistsXmlShowId = new ShowIdentity("artistshows", "xmlShow");
            public static readonly ShowIdentity ArtistsJsonShowWithCustomQueryParamId = new ShowIdentity("artistshows", "jsonCustomQueryParamShow");
        }

        public static class Views
        {
            public static readonly ViewIdentity[] AllViewIds;

            static Views()
            {
                AllViewIds = new[]
                {
                    ArtistsAlbumsViewId,
                    ArtistsNameNoValueViewId,
                    ArtistsTotalNumOfAlbumsViewId,
                    ArtistsNameAsKeyAndDocAsValueId
                };
            }

            public const string ArtistsViews =
                "{" +
                    "\"_id\": \"_design/artists\"," +
                    "\"language\": \"javascript\"," +
                    "\"lists\": {" +
                            "\"transformToHtml\": \"function(head, req){" +
                                    "provides('html',function(){" +
                                        "html = '<html><body><ol>';" +
                                        "while (row = getRow()) {" +
                                            "html += '<li>' + row.value.name + '</li>';" +
                                        "}" +
                                        "html += '</ol></body></html>';" +
                                        "return html;" +
                                    "});" +
                                "}\"," +
                            "\"transformToDoc\": \"function(head, req){" +
                                "provides('json',function(){" +
                                    "docs = [];" +
                                    "while (row = getRow()) {" +
                                        "docs.push(row.value);" +
                                    "}" +
                                    "send(JSON.stringify(docs));" +
                                "});" +
                            "}\"" +
                         "}," +
                         "\"views\": {" +
                        "\"albums\": {" +
                            "\"map\": \"function(doc) {  if(doc.$doctype !== 'artist') return;  emit(doc.name, doc.albums);}\"" +
                        "}," +
                        "\"name_no_value\": {" +
                            "\"map\": \"function(doc) {  if(doc.$doctype !== 'artist') return;  emit(doc.name, null);}\"" +
                        "}," +
                        "\"total_num_of_albums\": {" +
                            "\"map\": \"function(doc) {  if(doc.$doctype !== 'artist') return;  emit(null, doc.albums.length);}\"," +
                            "\"reduce\":\"_sum\"" +
                        "}," +
                        "\"name_as_key_and_doc_as_value\": {" +
                            "\"map\": \"function(doc) {  if(doc.$doctype !== 'artist') return;  emit(doc.name, doc);}\"" +
                        "}" +
                    "}" +
                "}";
            public static readonly ViewIdentity ArtistsAlbumsViewId = new ViewIdentity("artists", "albums");
            public static readonly ViewIdentity ArtistsNameNoValueViewId = new ViewIdentity("artists", "name_no_value");
            public static readonly ViewIdentity ArtistsTotalNumOfAlbumsViewId = new ViewIdentity("artists", "total_num_of_albums");
            public static readonly ViewIdentity ArtistsNameAsKeyAndDocAsValueId = new ViewIdentity("artists", "name_as_key_and_doc_as_value");

            public static class ListNames
            {
                public static readonly string TransformToHtmlListId = "transformToHtml";
                public static readonly string TransformToDocListId = "transformToDoc";
            }
        }

        public static class Attachments
        {
            public static class One
            {
                public const string Name = "att:1";
                public const string Content = "MyCouch, the simple asynchronous client for .Net";
                public static readonly string ContentType = HttpContentTypes.Text;
                public static readonly byte[] Bytes = MyCouchRuntime.DefaultEncoding.GetBytes(Content);
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