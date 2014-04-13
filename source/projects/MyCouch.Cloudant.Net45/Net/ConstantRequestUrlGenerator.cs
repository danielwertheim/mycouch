using System;
using EnsureThat;

namespace MyCouch.Net
{
    public class ConstantRequestUrlGenerator : IRequestUrlGenerator
    {
        public Uri DbUri { get; set; }
        public string DbName { get; set; }

        public ConstantRequestUrlGenerator(Uri dbUri, string dbName)
        {
            Ensure.That(dbUri, "dbUri").IsNotNull();
            Ensure.That(dbName, "dbName").IsNotNullOrWhiteSpace();

            DbUri = dbUri;
            DbName = dbName;
        }

        public virtual string Generate()
        {
            return DbUri.AbsoluteUri;
        }

        public virtual string Generate(string resourceName)
        {
            if (DbName != resourceName)
                throw new InvalidOperationException(ExceptionStrings.ConstantRequestUrlGenerationAgainstOtherDb);

            return Generate();
        }
    }
}