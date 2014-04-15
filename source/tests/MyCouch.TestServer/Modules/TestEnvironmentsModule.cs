using System.Collections.Generic;
using System.IO;
using Nancy;
using Nancy.Responses;

namespace MyCouch.TestServer.Modules
{
    public class TestEnvironmentsModule : NancyModule
    {
        private static readonly string EnvDataDirPath;

        static TestEnvironmentsModule()
        {
            EnvDataDirPath = GetEnvDataDirPath(AppSettings.EnvDataDirRelativePath);
            GenericFileResponse.SafePaths.Add(EnvDataDirPath);
        }

        private static string GetEnvDataDirPath(string envDataDirPathExpression)
        {
            var path = Path.GetFullPath(envDataDirPathExpression);
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(string.Format(
                    "The directory path expression '{0}' translated to '{1} could not be used to find an existing directory.",
                    envDataDirPathExpression, path));

            return path;
        }

        public TestEnvironmentsModule()
            : base("testenvironments")
        {
            Get["/{config}"] = p =>
            {
                var kv = (IDictionary<string, object>)p;
                var configName = kv.ContainsKey("config")
                    ? kv["config"].ToString()
                    : null;

                return configName == null
                    ? null
                    : new GenericFileResponse(GenerateJsonConfigFilePath(configName));
            };
        }

        private string GenerateJsonConfigFilePath(string configName)
        {
            return Path.Combine(EnvDataDirPath, configName + ".json");
        }
    }
}