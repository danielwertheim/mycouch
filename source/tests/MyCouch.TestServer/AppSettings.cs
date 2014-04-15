using System;
using System.Configuration;

namespace MyCouch.TestServer
{
    public static class AppSettings
    {
        private static readonly Lazy<string> LazyHostUri = new Lazy<string>(() => Read("host_uri"));
        private static readonly Lazy<string> LazyEnvDataDirRelativePath = new Lazy<string>(() => Read("env_data_dir_relative_path"));

        public static string HostUri { get { return LazyHostUri.Value; } }
        public static string EnvDataDirRelativePath { get { return LazyEnvDataDirRelativePath.Value; } }

        private static string Read(string key)
        {
            var envDataDirPathExpression = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(envDataDirPathExpression))
                throw new ConfigurationErrorsException(string.Format("AppSetting: '{0}' is missing value", key));

            return envDataDirPathExpression;
        }
    }
}