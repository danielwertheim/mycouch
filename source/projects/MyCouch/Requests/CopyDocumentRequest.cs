using EnsureThat;

namespace MyCouch.Requests
{
    public class CopyDocumentRequest : Request
    {
        /// <summary>
        /// The Id of the document to copy.
        /// </summary>
        public string SrcId { get; private set; }

        /// <summary>
        /// Optional, the Rev of the document to copy.
        /// </summary>
        public string SrcRev { get; private set; }

        /// <summary>
        /// The New Id of the new document being created as a copy.
        /// </summary>
        public string NewId { get; private set; }

        public CopyDocumentRequest(string srcId, string newId)
        {
            EnsureArg.IsNotNullOrWhiteSpace(srcId, nameof(srcId));
            EnsureArg.IsNotNullOrWhiteSpace(newId, nameof(newId));

            SrcId = srcId;
            NewId = newId;
        }

        public CopyDocumentRequest(string srcId, string srcRev, string newId)
        {
            EnsureArg.IsNotNullOrWhiteSpace(srcId, nameof(srcId));
            EnsureArg.IsNotNullOrWhiteSpace(srcRev, nameof(srcRev));
            EnsureArg.IsNotNullOrWhiteSpace(newId, nameof(newId));

            SrcId = srcId;
            SrcRev = srcRev;
            NewId = newId;
        }
    }
}