using EnsureThat;

namespace MyCouch.Requests.Factories
{
    public abstract class AttachmentHttpRequestFactoryBase : HttpRequestFactoryBase
    {
        protected AttachmentHttpRequestFactoryBase(IConnection connection) : base(connection) { }

        protected virtual string GenerateRequestUrl(string docId, string docRev, string attachmentName)
        {
            Ensure.That(docId, "docId")
                .WithExtraMessageOf(() => "PUT requests must have an id part of the URL.")
                .IsNotNullOrWhiteSpace();

            return string.Format("{0}/{1}/{2}{3}",
                Connection.Address,
                docId,
                attachmentName,
                docRev == null ? string.Empty : string.Concat("?rev=", docRev));
        }
    }
}