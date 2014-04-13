using MyCouch.EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public abstract class AttachmentHttpRequestFactoryBase : HttpRequestFactoryBase
    {
        protected ConstantRequestUrlGenerator RequestUrlGenerator { get; private set; }

        protected AttachmentHttpRequestFactoryBase(IDbClientConnection connection)
        {
            Ensure.That(connection, "connection").IsNotNull();

            RequestUrlGenerator = new ConstantRequestUrlGenerator(connection.Address, connection.DbName);
        }

        protected virtual string GenerateRequestUrl(string docId, string docRev, string attachmentName)
        {
            Ensure.That(docId, "docId")
                .WithExtraMessageOf(() => ExceptionStrings.PutRequestIsMissingIdInUrl)
                .IsNotNullOrWhiteSpace();

            return string.Format("{0}/{1}/{2}{3}",
                RequestUrlGenerator.Generate(),
                docId,
                attachmentName,
                docRev == null ? string.Empty : string.Concat("?rev=", docRev));
        }
    }
}