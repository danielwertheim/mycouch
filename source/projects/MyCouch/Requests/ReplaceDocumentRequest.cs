using EnsureThat;

namespace MyCouch.Requests
{
    public class ReplaceDocumentRequest : Request
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
        /// The Id of the document being replaced.
        /// </summary>
        public string TrgId { get; private set; }

        /// <summary>
        /// The Rev of the document being replaced.
        /// </summary>
        public string TrgRev { get; private set; }

        public ReplaceDocumentRequest(string srcId, string trgId, string trgRev)
        {
            EnsureArg.IsNotNullOrWhiteSpace(srcId, nameof(srcId));
            EnsureArg.IsNotNullOrWhiteSpace(trgId, nameof(trgId));
            EnsureArg.IsNotNullOrWhiteSpace(trgRev, nameof(trgRev));

            SrcId = srcId;
            TrgId = trgId;
            TrgRev = trgRev;
        }

        public ReplaceDocumentRequest(string srcId, string srcRev, string trgId, string trgRev)
        {
            EnsureArg.IsNotNullOrWhiteSpace(srcId, nameof(srcId));
            EnsureArg.IsNotNullOrWhiteSpace(srcRev, nameof(srcRev));
            EnsureArg.IsNotNullOrWhiteSpace(trgId, nameof(trgId));
            EnsureArg.IsNotNullOrWhiteSpace(trgRev, nameof(trgRev));

            SrcId = srcId;
            SrcRev = srcRev;
            TrgId = trgId;
            TrgRev = trgRev;
        }
    }
}