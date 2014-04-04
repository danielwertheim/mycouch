using System;
using EnsureThat;

namespace MyCouch.Net
{
    public class AppendingDbRequestUrlGenerator : IDbRequestUrlGenerator
    {
        private readonly string _basePath;

        public AppendingDbRequestUrlGenerator(Uri baseAddress)
        {
            Ensure.That(baseAddress, "baseAddress").IsNotNull();

            _basePath = baseAddress.AbsoluteUri.TrimEnd('/', '?');
        }

        public string Generate(string dbName)
        {
            return string.Format("{0}/{1}", _basePath, dbName);
        }
    }
}