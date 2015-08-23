using System;
using System.Collections.Generic;
using System.Linq;
using EnsureThat;

namespace MyCouch.Requests
{
#if !PCL
    [Serializable]
#endif
    public class BulkRequest : Request
    {
        protected List<string> Documents { get; private set; }

        public virtual bool IsEmpty { get { return !Documents.Any(); } }

        public BulkRequest()
        {
            Documents = new List<string>();
        }

        /// <summary>
        /// Returns the documents that are included in this bulk request.
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetDocuments()
        {
            return Documents.ToArray();
        }

        /// <summary>
        /// Includes documents for insert, updates or deletes. For deletes
        /// you can also use <see cref="Delete(string, string)"/>.
        /// </summary>
        /// <param name="docs"></param>
        /// <returns></returns>
        public virtual BulkRequest Include(params string[] docs)
        {
            Documents.AddRange(docs);

            return this;
        }

        /// <summary>
        /// Includes documents for deletion.
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public virtual BulkRequest Delete(params DocumentHeader[] headers)
        {
            Ensure.That(headers, "headers").HasItems();

            foreach (var header in headers)
                Delete(header.Id, header.Rev);

            return this;
        }

        /// <summary>
        /// Includes a document for deletion.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        public virtual BulkRequest Delete(string id, string rev)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(rev, "rev").IsNotNullOrWhiteSpace();

            Include(string.Format("{{\"_id\":\"{0}\",\"_rev\":\"{1}\",\"_deleted\":true}}", id, rev));

            return this;
        }
    }
}