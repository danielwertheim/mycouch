using System;
using EnsureThat;

namespace MyCouch.Requests
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class ReplaceDocumentRequest : IRequest
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
            Ensure.That(srcId, "srcId").IsNotNullOrWhiteSpace();
            Ensure.That(trgId, "trgId").IsNotNullOrWhiteSpace();
            Ensure.That(trgRev, "trgRev").IsNotNullOrWhiteSpace();

            SrcId = srcId;
            TrgId = trgId;
            TrgRev = trgRev;
        }

        public ReplaceDocumentRequest(string srcId, string srcRev, string trgId, string trgRev)
        {
            Ensure.That(srcId, "srcId").IsNotNullOrWhiteSpace();
            Ensure.That(srcRev, "srcRev").IsNotNullOrWhiteSpace();
            Ensure.That(trgId, "trgId").IsNotNullOrWhiteSpace();
            Ensure.That(trgRev, "trgRev").IsNotNullOrWhiteSpace();

            SrcId = srcId;
            SrcRev = srcRev;
            TrgId = trgId;
            TrgRev = trgRev;
        }
    }
}