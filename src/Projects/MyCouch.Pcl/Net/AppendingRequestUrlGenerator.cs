using System;
using MyCouch.EnsureThat;

namespace MyCouch.Net
{
    public class AppendingRequestUrlGenerator : IRequestUrlGenerator
    {
        private readonly string _basePath;

        public AppendingRequestUrlGenerator(Uri baseAddress)
        {
            Ensure.That(baseAddress, "baseAddress").IsNotNull();

            _basePath = baseAddress.AbsoluteUri.TrimEnd('/', '?');
        }

        public string Generate(string resourceName)
        {
            return string.Format("{0}/{1}", _basePath, resourceName);
        }
    }
}