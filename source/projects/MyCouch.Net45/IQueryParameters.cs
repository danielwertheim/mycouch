using System.Collections.Generic;

namespace MyCouch
{
    /// <summary>
    /// The different common query parameters that can be specified
    /// when performing a query against a View-index.
    /// </summary>
    public interface IQueryParameters
    {
        /// <summary>
        /// Identitfies the view that the Query will be
        /// performed against.
        /// </summary>
        ViewIdentity ViewIdentity { get; }

        /// <summary>
        /// Used to set custom accept header values.
        /// Applicable e.g. when specifying a <see cref="ListName"/>
        /// that returns e.g. HTML.
        /// </summary>
        string[] Accepts { get; set; }

        /// <summary>
        /// Indicates if any <see cref="Accept"/> has been specified.
        /// </summary>
        bool HasAccepts { get; }

        /// <summary>
        /// Allow the results from a stale view to be used.
        /// </summary>
        Stale? Stale { get; set; }

        /// <summary>
        /// Include the full content of the documents in the return.
        /// </summary>
        bool? IncludeDocs { get; set; }

        /// <summary>
        /// Return the documents in descending by key order.
        /// </summary>
        bool? Descending { get; set; }

        /// <summary>
        /// Return only documents that match the specified key.
        /// </summary>
        object Key { get; set; }

        /// <summary>
        /// Returns only documents that matches any of the specified keys.
        /// </summary>
        object[] Keys { get; set; }

        /// <summary>
        /// Indicates if any <see cref="Keys"/> has been specified.
        /// </summary>
        bool HasKeys { get; }

        /// <summary>
        /// Return records starting with the specified key.
        /// </summary>
        object StartKey { get; set; }

        /// <summary>
        /// Return records starting with the specified document ID.
        /// </summary>
        string StartKeyDocId { get; set; }

        /// <summary>
        /// Stop returning records when the specified key is reached.
        /// </summary>
        object EndKey { get; set; }

        /// <summary>
        /// Stop returning records when the specified document ID is reached.
        /// </summary>
        string EndKeyDocId { get; set; }

        /// <summary>
        /// Specifies whether the specified end key should be included in the result.
        /// </summary>
        bool? InclusiveEnd { get; set; }

        /// <summary>
        /// Skip this number of records before starting to return the results.
        /// </summary>
        int? Skip { get; set; }

        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        int? Limit { get; set; }

        /// <summary>
        /// Use the reduction function.
        /// </summary>
        bool? Reduce { get; set; }

        /// <summary>
        /// Include the update sequence in the generated results.
        /// </summary>
        bool? UpdateSeq { get; set; }

        /// <summary>
        /// The group option controls whether the reduce function reduces to a set of distinct keys or to a single result row.
        /// </summary>
        bool? Group { get; set; }

        /// <summary>
        /// Specify the group level to be used.
        /// </summary>
        int? GroupLevel { get; set; }

        /// <summary>
        /// Specify if you want to target a specific list in the view.
        /// </summary>
        string ListName { get; set; }

        /// <summary>
        /// Additional custom query string parameters.
        /// </summary>
        IDictionary<string, object> CustomQueryParameters { get; set; }

        /// <summary>
        /// Indicates if there are any <see cref="CustomQueryParameters"/> or not.
        /// </summary>
        bool HasCustomQueryParameters { get; }
    }
}