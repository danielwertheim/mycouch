using System.Collections.Generic;
using System.Linq;
using EnsureThat;

namespace MyCouch.Requests
{
    public class PurgeRequest : Request
    {
        public Dictionary<string, string[]> SeqsById { get; private set; }

        public PurgeRequest()
        {
            SeqsById = new Dictionary<string, string[]>();
        }

        /// <summary>
        /// Includes documents for purge
        /// </summary>
        /// <param name="id">Document identifier</param>
        /// <param name="revs">Document revisions</param>
        /// <returns></returns>
        public virtual PurgeRequest Include(string id, params string[] revs)  
        {
            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));
            EnsureArg.HasItems(revs, nameof(revs));

            if (!SeqsById.TryGetValue(id, out string[] foundRevs)) SeqsById[id] = revs;
            else SeqsById[id] = foundRevs.Union(revs).ToArray();

            return this;
        }

        /// <summary>
        /// Includes documents for purge
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public virtual PurgeRequest Include(params DocumentHeader[] headers)
        {
            EnsureArg.HasItems(headers, nameof(headers));

            foreach (var header in headers)
                Include(header.Id, header.Rev);

            return this;
        }
    }
}