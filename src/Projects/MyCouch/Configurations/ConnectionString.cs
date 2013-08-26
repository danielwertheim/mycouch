#if !NETFX_CORE
using System;
using System.Configuration;
using EnsureThat;

namespace MyCouch.Configurations
{
    public static class ConnectionString
    {
        public static string Get(string connectionStringName)
        {
            Ensure.That(connectionStringName, "connectionStringName").IsNotNullOrWhiteSpace();

            var machineSpecificName = string.Concat(connectionStringName, "@", Environment.MachineName);
            var config = ConfigurationManager.ConnectionStrings[machineSpecificName]
                ?? ConfigurationManager.ConnectionStrings[connectionStringName];

            return config == null
                ? connectionStringName
                : config.ConnectionString;
        }
    }
}
#endif