using System.Collections.Concurrent;
using System.IO;

#if NETFX_CORE
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
#endif

namespace MyCouch.Testing.TestData
{
    public static class JsonTestData
    {
        private static readonly ConcurrentDictionary<string, string> Cache;

        static JsonTestData()
        {
            Cache = new ConcurrentDictionary<string, string>();
        }

        public static string ViewQueryAlbumRows
        {
            get
            {
                return ReadFile("ViewQueryAlbumRows.json");
            }
        }

        public static string ViewQueryAllDocRows
        {
            get
            {
                return ReadFile("ViewQueryAllDocRows.json");
            }
        }

        public static string ViewQueryComplexKeysRows
        {
            get
            {
                return ReadFile("ViewQueryComplexKeysRows.json");
            }
        }

        public static string ViewQuerySingleValueKeysRows
        {
            get
            {
                return ReadFile("ViewQuerySingleValueKeysRows.json");
            }
        }

        private static string ReadFile(string name)
        {
#if !NETFX_CORE
            var filePath = Path.Combine(string.Concat(typeof(JsonTestData).Name, "Files"), name);

            return Cache.GetOrAdd(name, File.ReadAllText(filePath));
#else
            return Cache.GetOrAdd(name, WinRtStyleOfDoingASimpleRead(name).Result);
#endif
        }
#if NETFX_CORE
        private static async Task<string> WinRtStyleOfDoingASimpleRead(string name)
        {
            var folder = await Package.Current.InstalledLocation.GetFolderAsync(Path.Combine(typeof(JsonTestData).GetTypeInfo().Assembly.GetName().Name, string.Concat(typeof(JsonTestData).Name, "Files")));
            var file = await folder.GetFileAsync(name);
            
            return await FileIO.ReadTextAsync(file);
        }
#endif
    }
}
