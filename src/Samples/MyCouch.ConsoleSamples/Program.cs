using System;
using System.Collections.Generic;

namespace MyCouch.ConsoleSamples
{
    //http://wiki.apache.org/couchdb/HTTP_view_API
    //http://wiki.apache.org/couchdb/Introduction_to_CouchDB_views
    public class Program
    {
        static void Main(string[] args)
        {
            using (var client = new Client("http://127.0.0.1:5984/test"))
            {
                //CreateDb(client);
                //CreateView(client);

                //DocCrudUsingNonTypedApi(client, false);
                //DocCrudUsingEntityApi(client, false);
                //ViewsUsingNonTypedQueryApi(client);
                //ViewsUsingNonTypedRunQueryApi(client);
                //ViewsUsingTypedRunQueryApi(client);

                //DeleteDb(client);
            }

            Console.ReadKey();
        }

        private static void CreateDb(IClient client)
        {
            Console.WriteLine("***** ***** CreateDb ***** *****");
            var r = client.Databases.Put("test");
            Console.WriteLine(r);
        }

        private static void CreateView(IClient client)
        {
            Console.WriteLine("***** ***** CreateView ***** *****");
            var r = client.Documents.Post(Views.ArtistsAlbums);
            Console.WriteLine(r);
        }

        private static void DeleteDb(IClient client)
        {
            Console.WriteLine("***** ***** DeleteDb ***** *****");
            var r = client.Databases.Delete("test");
            Console.WriteLine(r);
        }

        private static void DocCrudUsingNonTypedApi(IClient client, bool deleteDocs)
        {
            Console.WriteLine("***** ***** DocCrudUsingNonTypedApi ***** *****");
            var post1 = client.Documents.PostAsync(SampleData.Doc1).Result;
            Console.WriteLine(post1);

            var post2 = client.Documents.Post(SampleData.Doc2);
            Console.WriteLine(post2);

            var get1 = client.Documents.GetAsync(post1.Id).Result;
            Console.WriteLine(get1);

            var get2 = client.Documents.Get(post2.Id);
            Console.WriteLine(get2);

            var kv1 = client.Serializer.Deserialize<IDictionary<string, dynamic>>(get1.Content);
            kv1["year"] = 2000;
            var docUpd1 = client.Serializer.Serialize(kv1);
            var put1 = client.Documents.PutAsync(get1.Id, docUpd1).Result;

            var kv2 = client.Serializer.Deserialize<IDictionary<string, dynamic>>(get2.Content);
            kv2["year"] = 2001;
            var docUpd2 = client.Serializer.Serialize(kv2);
            var put2 = client.Documents.Put(get2.Id, docUpd2);

            if (!deleteDocs)
                return;

            Console.WriteLine(client.Documents.DeleteAsync(put1.Id, put1.Rev).Result);
            Console.WriteLine(client.Documents.Delete(put2.Id, put2.Rev));
        }

        private static void DocCrudUsingEntityApi(IClient client, bool deleteDocs)
        {
            Console.WriteLine("***** ***** DocCrudUsingEntityApi ***** *****");
            var post3 = client.Documents.PostAsync(SampleData.Doc3).Result;
            Console.WriteLine(post3);

            var post4 = client.Documents.Post(SampleData.Doc4);
            Console.WriteLine(post4);

            var get3 = client.Documents.GetAsync<Artist>(post3.Id).Result;
            Console.WriteLine(get3);

            var get4 = client.Documents.Get<Artist>(post4.Id);
            Console.WriteLine(get4);

            get3.Entity.Albums = new List<Album>(get3.Entity.Albums) { new Album { Name = "Test" } }.ToArray();
            var put3 = client.Documents.PutAsync(get3.Entity).Result;

            get4.Entity.Albums = new List<Album>(get4.Entity.Albums) { new Album { Name = "Test" } }.ToArray();
            var put4 = client.Documents.Put(get4.Entity);

            if (!deleteDocs)
                return;

            Console.WriteLine(client.Documents.DeleteAsync(put3.Entity).Result);
            Console.WriteLine(client.Documents.Delete(put4.Entity));
        }

        private static void ViewsUsingNonTypedQueryApi(IClient client)
        {
            Console.WriteLine("***** ***** ViewsUsingNonTypedQueryApi ***** *****");
            var result = client.Views.Query<string>("artists", "albums", cfg => cfg.Limit(5).Reduce(false));
            Console.WriteLine(result);
        }

        private static void ViewsUsingNonTypedRunQueryApi(IClient client)
        {
            Console.WriteLine("***** ***** ViewsUsingNonTypedRunQueryApi ***** *****");
            var query = client.Views.CreateQuery("artists", "albums").Configure(o => o.Limit(5).Reduce(false));
            var result = client.Views.RunQuery<string[]>(query);
            Console.WriteLine(result);
        }

        private static void ViewsUsingTypedRunQueryApi(IClient client)
        {
            Console.WriteLine("***** ***** ViewsUsingNonTypedRunQueryApi ***** *****");
            var query = client.Views.CreateQuery("artists", "albums").Configure(o => o.Limit(5).Reduce(false));
            var result = client.Views.RunQuery<Album[]>(query);
            Console.WriteLine(result);
        }
    }

    internal static class Views
    {
        internal const string ArtistsAlbums =
            "{" +
                "\"_id\": \"_design/artists\"," +
                "\"language\": \"javascript\"," +
                "\"views\": {" +
                    "\"albums\": {" +
                        "\"map\": \"function(doc) {  if(!doc.$doctype === 'artist') return;  emit(doc.name, doc.albums);}\"" +
                    "}" +
                "}" +
            "}";
    }

    public class Artist
    {
        //Could be _id, ArtistId, DocumentId, EntityId, Id (you can change this convention)
        public string ArtistId { get; set; }

        //Could be _rev, ArtistRev, DocumentRev, EntityRev, Rev (you can change this convention)
        public string ArtistRev { get; set; }

        public string Name { get; set; }

        public Album[] Albums { get; set; }
    }

    public class Album
    {
        public string Name { get; set; }
    }

    internal static class SampleData
    {
        internal const string Doc1 = "{\"_id\": \"1\", \"$doctype\": \"artist\", \"name\": \"Fake artist 1\", \"albums\":[{\"name\": \"Greatest fakes #1\"}]}";
        internal const string Doc2 = "{\"_id\": \"2\", \"$doctype\": \"artist\", \"name\": \"Fake artist 2\", \"albums\":[{\"name\": \"Greatest fakes #2\"}]}";

        internal static readonly Artist Doc3 = new Artist
        {
            ArtistId = "3",
            Name = "Fake artist 3",
            Albums = new[]
            {
                new Album { Name = "Greatest fakes #3" }
            }
        };

        internal static readonly Artist Doc4 = new Artist
        {
            ArtistId = "4",
            Name = "Fake artist 4",
            Albums = new[]
            {
                new Album { Name = "Greatest fakes #4" }
            }
        };
    }
}
