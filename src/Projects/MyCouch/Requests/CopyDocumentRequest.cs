using System;
using EnsureThat;

namespace MyCouch.Requests
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class CopyDocumentRequest : IRequest
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
            Ensure.That(srcId, "srcId").IsNotNullOrWhiteSpace();
            Ensure.That(newId, "newId").IsNotNullOrWhiteSpace();

            SrcId = srcId;
            NewId = newId;
        }

        public CopyDocumentRequest(string srcId, string srcRev, string newId)
        {
            Ensure.That(srcId, "srcId").IsNotNullOrWhiteSpace();
            Ensure.That(srcRev, "srcRev").IsNotNullOrWhiteSpace();
            Ensure.That(newId, "newId").IsNotNullOrWhiteSpace();

            SrcId = srcId;
            SrcRev = srcRev;
            NewId = newId;
        }
    }
}