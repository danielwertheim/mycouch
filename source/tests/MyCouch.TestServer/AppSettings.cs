using System;
using System.Configuration;

namespace MyCouch.TestServer
{
    public static class AppSettings
    {
        private static readonly Lazy<string> LazyHostUri = new Lazy<string>(() => Read("host_uri"));
        private static readonly Lazy<string> LazyTestEnvironmentsRelativePath = new Lazy<string>(() => Read("testenvironments_relative_path"));

        public static string HostUri { get { return LazyHostUri.Value; } }
        public static string TestEnvironmentsRelativePath { get { return LazyTestEnvironmentsRelativePath.Value; } }

        private static string Read(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(value))
                throw new ConfigurationErrorsException(string.Format("AppSetting: '{0}' is missing value", key));

            return value;
        }
    }
}