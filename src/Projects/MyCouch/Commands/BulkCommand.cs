using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyCouch.Commands
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class BulkCommand : ICommand
    {
        protected readonly List<string> Docs;

        public BulkCommand()
        {
            Docs = new List<string>();
        }

        /// <summary>
        /// Includes documents for insert, updates or deletes. For deletes
        /// you can also use <see cref="Delete"/>.
        /// </summary>
        /// <param name="docs"></param>
        /// <returns></returns>
        public virtual BulkCommand Include(params string[] docs)
        {
            Docs.AddRange(docs);

            return this;
        }

        /// <summary>
        /// Includes a document for deletion.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        public virtual BulkCommand Delete(string id, string rev)
        {
            Include(string.Format("{{\"_id\":\"{0}\",\"_rev\":\"{1}\",\"_deleted\":true}}", id, rev));

            return this;
        }

        public virtual string ToJson()
        {
            var sb = new StringBuilder();

            using (var wr = new StringWriter(sb))
            {
                wr.Write("{\"docs\":[");
                for (var i = 0; i < Docs.Count; i++)
                {
                    wr.Write(Docs[i]);
                    if (i < Docs.Count - 1)
                        wr.Write(",");
                }
                wr.Write("]}");
            }

            return sb.ToString();
        }
    }
}