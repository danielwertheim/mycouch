using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MyCouch;
using MyCouch.Requests;

namespace MrLab
{
    class Program
    {
        static void Main(string[] args)
        {
            Test().Wait();
        }

        private async static Task Test()
        {
            using (var client = new MyCouchClient("http://sa:test@localhost:5984/", "foo"))
            {
                var db = await client.Database.PutAsync();

                var put = await client.Entities.PutAsync(
                    new Person { Id = "persons/1", Name = "Daniel" });

                var get = await client.Entities.GetAsync<Person>(put.Id);
            }
        }
    }

    public class Person
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
