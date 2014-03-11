using System;
using Microsoft.Owin.Hosting;

namespace MyCouch.TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = AppSettings.HostUri;

            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("Running on {0}", url);
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }
    }
}