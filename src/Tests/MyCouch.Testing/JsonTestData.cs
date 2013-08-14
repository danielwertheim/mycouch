using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
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
                return ReadFile("ViewQueryAlbums.json");
            }
        }

        public static string ViewQueryAllDocsResult
        {
            get
            {
                return ReadFile("ViewQueryAllDocsResult.json");
            }
        }

        private static string ReadFile(string name)
        {
#if !WinRT
            var filePath = Path.Combine(GenerateFilesFolderName(), name);

            return Cache.GetOrAdd(name, File.ReadAllText(filePath));
#else
            return Cache.GetOrAdd(name, WinRtStyleOfDoingASimpleRead(name).Result);
#endif
        }
#if WinRT
        private static async Task<string> WinRtStyleOfDoingASimpleRead(string name)
        {
            var folder = await Package.Current.InstalledLocation.GetFolderAsync(GenerateFilesFolderName());
            var file = await folder.GetFileAsync(name);
            
            return await FileIO.ReadTextAsync(file);
        }
#endif
        private static string GenerateFilesFolderName()
        {
            return string.Concat(typeof(JsonTestData).Name, "Files");
        }
    }
}
