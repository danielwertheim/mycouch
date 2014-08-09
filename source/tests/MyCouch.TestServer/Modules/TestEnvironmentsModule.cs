using System.IO;
using EnsureThat;
using Nancy;
using Nancy.Responses;

namespace MyCouch.TestServer.Modules
{
    public class TestEnvironmentsModule : NancyModule
    {
        private static readonly string TestEnvironmentsFullPath;

        static TestEnvironmentsModule()
        {
            TestEnvironmentsFullPath = GetFullPath(AppSettings.TestEnvironmentsRelativePath);
            GenericFileResponse.SafePaths.Add(TestEnvironmentsFullPath);
        }

        private static string GetFullPath(string relativePath)
        {
            Ensure.That(relativePath, "relativePath").IsNotNullOrWhiteSpace();

            var fullPath = Path.GetFullPath(relativePath);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Could not find file", relativePath);

            return fullPath;
        }

        public TestEnvironmentsModule()
            : base("testenvironments")
        {
            Get["/"] = p => new GenericFileResponse(TestEnvironmentsFullPath);
        }
    }
}