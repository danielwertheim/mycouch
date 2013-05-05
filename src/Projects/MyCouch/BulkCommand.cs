using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyCouch
{
    [Serializable]
    public class BulkCommand
    {
        protected readonly List<string> Docs;

        public BulkCommand()
        {
            Docs = new List<string>();
        }

        public virtual BulkCommand Include(params string[] docs)
        {
            Docs.AddRange(docs);

            return this;
        }

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