using System.Runtime.CompilerServices;
using MyCouch;
using MyCouch.Requests;

namespace MrLab
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (var client = new MyCouchClient("http://sa:test@localhost:5984/foo"))
            //{
                //client.Database.PutAsync().Wait();

                //var postEntity = client.Entities.PostAsync(new Doc
                //{
                //    Item = new Nested { EntityId = "Test" }
                //}).Result;

                //var getEntity = client.Entities.GetAsync<Doc>(postEntity.Id).Result;
                //var getJson = client.Documents.GetAsync(postEntity.Id).Result;

                //var all = client.Views.QueryAsync<Doc>(new QueryViewRequest("test", "all")).Result;
            //}
        }

        public class Doc
        {
            public string DocumentId { get; set; }
            public string DocumentRev { get; set; }
            public Nested Item { get; set; }
        }

        public class Nested
        {
            public string EntityId { get; set; }
        }
    }
}
