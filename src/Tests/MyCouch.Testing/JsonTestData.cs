using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
#if WinRT
using Windows.ApplicationModel;
using Windows.Storage;
#endif

namespace MyCouch.Testing
{
    public static class JsonTestData
    {
        private static readonly ConcurrentDictionary<string, string> Cache;

        static JsonTestData()
        {
            Cache = new ConcurrentDictionary<string, string>();
        }

        public static string ViewQueryAlbums
        {
            get
            {
                return ReadFile("ViewQueryAlbums");
            }
        }

        public static string ViewQueryAllDocsResult
        {
            get
            {
                return ReadFile("ViewQueryAllDocsResult");
            }
        }

        private static string ReadFile(string name)
        {
#if !WinRT
            return Cache.GetOrAdd(name, File.ReadAllText(Path.Combine(name, ".json")));
#else
            return Cache.GetOrAdd(name, WinRtStyleOfDoingASimpleRead(name).Result);
#endif
        }
#if WinRT
        private static async Task<string> WinRtStyleOfDoingASimpleRead(string name)
        {
            var folder = Package.Current.InstalledLocation;
            var file = await folder.GetFileAsync(Path.Combine(name, ".json"));
            
            return await FileIO.ReadTextAsync(file);
        }
#endif
    }
}
