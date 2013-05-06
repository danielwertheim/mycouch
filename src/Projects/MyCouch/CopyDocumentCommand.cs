using System;

namespace MyCouch
{
    [Serializable]
    public class CopyDocumentCommand
    {
        /// <summary>
        /// The Id of the document to copy.
        /// </summary>
        public string SrcId { get; set; }

        /// <summary>
        /// Optional, lets you specify a specific document revision to copy.
        /// </summary>
        public string SrcRev { get; set; }

        /// <summary>
        /// The New Id of the new document being created as a copy.
        /// </summary>
        public string NewId { get; set; }
    }
}