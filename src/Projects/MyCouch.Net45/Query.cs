using System;
using MyCouch.Requests;

namespace MyCouch
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class Query
    {
        protected QueryViewRequest Request { get; private set; }

        public ViewIdentity ViewIdentity { get { return Request.ViewIdentity; } }

        public Query(string designDocument, string viewName)
            : this(new ViewIdentity(designDocument, viewName)) { }

        public Query(ViewIdentity viewIdentity)
        {
            Request = new QueryViewRequest(viewIdentity);
        }

        public static implicit operator QueryViewRequest(Query query)
        {
            return query.Request;
        }

        /// <summary>
        /// Allow the results from a stale view to be used.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Query Stale(Stale value)
        {
            Request.Stale = value;

            return this;
        }
        /// <summary>
        /// Include the full content of the documents in the return;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Query IncludeDocs(bool value)
        {
            Request.IncludeDocs = value;

            return this;
        }
        /// <summary>
        /// Return the documents in descending by key order.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Query Descending(bool value)
        {
            Request.Descending = value;

            return this;
        }
        /// <summary>
        /// Return only documents that match the specified key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Query Key<T>(T value)
        {
            Request.Key = value;

            return this;
        }
        /// <summary>
        /// Return only documents that match the specified complex-key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Query Key<T>(T[] value)
        {
            Request.Key = value;

            return this;
        }
        /// <summary>
        /// Returns only documents that matches any of the specified keys.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Query Keys<T>(params T[] value)
        {
            Request.Keys = value as object[];

            return this;
        }
        /// <summary>
        /// Return records starting with the specified key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Query StartKey<T>(T value)
        {
            Request.StartKey = value;

            return this;
        }
        /// <summary>
        /// Return records starting with the specified complex-key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Query StartKey<T>(T[] value)
        {
            Request.StartKey = value;

            return this;
        }
        /// <summary>
        /// Return records starting with the specified document ID.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Query StartKeyDocId(string value)
        {
            Request.StartKeyDocId = value;

            return this;
        }
        /// <summary>
        /// Stop returning records when the specified key is reached.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Query EndKey<T>(T value)
        {
            Request.EndKey = value;

            return this;
        }
        /// <summary>
        /// Stop returning records when the specified complex-key is reached.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Query EndKey<T>(T[] value)
        {
            Request.EndKey = value;

            return this;
        }
        /// <summary>
        /// Stop returning records when the specified document ID is reached.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Query EndKeyDocId(string value)
        {
            Request.EndKeyDocId = value;

            return this;
        }
        /// <summary>
        /// Specifies whether the specified end key should be included in the result.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Query InclusiveEnd(bool value)
        {
            Request.InclusiveEnd = value;

            return this;
        }
        /// <summary>
        /// Skip this number of records before starting to return the results.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Query Skip(int value)
        {
            Request.Skip = value;

            return this;
        }
        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Query Limit(int value)
        {
            Request.Limit = value;

            return this;
        }
        /// <summary>
        /// Use the reduction function.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Query Reduce(bool value)
        {
            Request.Reduce = value;

            return this;
        }
        /// <summary>
        /// Include the update sequence in the generated results.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Query UpdateSeq(bool value)
        {
            Request.UpdateSeq = value;

            return this;
        }
        /// <summary>
        /// The group option controls whether the reduce function reduces to a set of distinct keys or to a single result row.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Query Group(bool value)
        {
            Request.Group = value;

            return this;
        }
        /// <summary>
        /// Specify the group level to be used.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Query GroupLevel(int value)
        {
            Request.GroupLevel = value;

            return this;
        }
    }
}