using System;

namespace MyCouch.Requests
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class GetChangesRequest : Request
    {
        /// <summary>
        /// Select the type of changes feed to consume.
        /// </summary>
        public ChangesFeed Feed { get; set; }
        /// <summary>
        /// Specifies how many revisions are returned in the changes array.
        /// The default, main_only, will only return the current “winning” revision;
        /// all_docs will return all leaf revisions (including conflicts and deleted former conflicts.)
        /// </summary>
        public ChangesStyle Style { get; set; }
        /// <summary>
        /// Start the results from the change immediately after the given sequence number.
        /// </summary>
        public long? Since { get; set; }
        /// <summary>
        /// Limit number of result rows to the specified value.
        /// </summary>
        /// <remarks>Using 0 here has the same effect as 1: get a single result row</remarks>
        public int? Limit { get; set; }
        /// <summary>
        /// Return the change results in descending sequence order (most recent change first)
        /// </summary>
        public bool? Descending { get; set; }
        /// <summary>
        /// Set a millisecond value to have CouchDbReport to send a
        /// newline at every tick where the length between the ticks
        /// is the value you define.
        /// </summary>
        public int? Heartbeat { get; set; }
        /// <summary>
        /// Maximum period in milliseconds to wait for a change before the response is sent,
        /// even if there are no results.
        /// </summary>
        /// <remarks>
        /// Only applicable for longpoll or continuous feeds.
        /// 60000 is also the default maximum timeout to prevent undetected dead connections.
        /// </remarks>
        public int? Timeout { get; set; }
        /// <summary>
        /// Determines if the response should include the docs
        /// that are affected by the change(s).
        /// </summary>
        public bool? IncludeDocs { get; set; }
        /// <summary>
        /// Set to a <example><![CDATA[designdoc/filtername]]></example> to reference a filter function
        /// from a design document to selectively get updates. 
        /// </summary>
        public string Filter { get; set; }
    }
}