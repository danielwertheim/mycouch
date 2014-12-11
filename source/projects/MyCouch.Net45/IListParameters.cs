
using System.Collections.Generic;
namespace MyCouch
{
    /// <summary>
    /// The different common query parameters that can be specified
    /// when performing a query against a List-function.
    /// </summary>
    public interface IListParameters
    {
        /// <summary>
        /// Identitfies the list function that the Query will be
        /// performed against.
        /// </summary>
        ListIdentity ListIdentity { get; }
        /// <summary>
        /// The view to be used for the list query
        /// </summary>
        string ViewName { get; set; }
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

        //Plan to add other view query options in simimar pattern as above.

        IDictionary<string, object> AdditionalQueryParameters { get; set; }
        bool HasAdditionalQueryParameters { get; }
    }
}
