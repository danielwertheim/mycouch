using System;

namespace MyCouch.Cloudant
{
    public class CloudantClient : Client, ICloudantClient
    {
        public CloudantClient(string url) : base(url) {}
        public CloudantClient(Uri uri) : base(uri) {}
        public CloudantClient(IConnection connection, ClientBootstraper bootstraper = null) : base(connection, bootstraper) {}
    }
}