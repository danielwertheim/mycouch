using System;
using MyCouch.EnsureThat;

namespace MyCouch.Requests
{
    public class PutDocumentRequest : Request
    {
        public string Id { get; private set; }
        public string Rev { get; private set; }
        public string Content { get; private set; }
        public bool Batch { get; set; }

        /// <summary>
        /// Used for creating new documents.
        /// How-ever, If <paramref name="content"/> contains _rev, it will be
        /// possible to update documents.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        public PutDocumentRequest(string id, string content)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(content, "content").IsNotNullOrWhiteSpace();

            Initialize(id, null, content);
        }

        /// <summary>
        /// Used for updating documents.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <param name="content"></param>
        public PutDocumentRequest(string id, string rev, string content)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(rev, "rev").IsNotNullOrWhiteSpace();
            Ensure.That(content, "content").IsNotNullOrWhiteSpace();

            Initialize(id, rev, content);
        }

        private void Initialize(string id, string rev, string content)
        {
            Batch = false;
            Id = id;
            Rev = rev;
            Content = content;
        }

        /// <summary>
        /// Inteded for CREATES, but if <paramref name="content"/> contains
        /// _rev, it can be used for UPDATES as well.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static PutDocumentRequest ForCreate(string id, string content, Action<PutDocumentRequest> cfg = null)
        {
            var r = new PutDocumentRequest(id, content);

            if (cfg != null)
                cfg(r);

            return r;
        }

        /// <summary>
        /// Creates request instances used to update documents.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <param name="content"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static PutDocumentRequest ForUpdate(string id, string rev, string content, Action<PutDocumentRequest> cfg = null)
        {
            var r = new PutDocumentRequest(id, rev, content);

            if (cfg != null)
                cfg(r);

            return r;
        }
    }
}