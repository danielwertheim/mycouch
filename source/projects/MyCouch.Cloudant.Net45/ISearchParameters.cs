using System.Collections.Generic;

namespace MyCouch.Cloudant
{
    /// <summary>
    /// The different common search parameters that can be specified
    /// when performing a query against a Search-index.
    /// </summary>
    public interface ISearchParameters
    {
        /// <summary>
        /// Identitfies the Search index that this request will be
        /// performed against.
        /// </summary>
        SearchIndexIdentity IndexIdentity { get; }

        /// <summary>
        /// The Lucene expression that will be used to query the index.
        /// </summary>
        string Expression { get; set; }

        /// <summary>
        /// Allow the results from a stale search index to be used.
        /// </summary>
        Stale? Stale { get; set; }

        /// <summary>
        /// A bookmark that was received from a previous search. This
        /// allows you to page through the results. If there are no more
        /// results after the bookmark, you will get a response with an
        /// empty rows array and the same bookmark. That way you can
        /// determine that you have reached the end of the result list.
        /// </summary>
        string Bookmark { get; set; }

        /// <summary>
        /// Sort expressions used to sort the output.
        /// </summary>
        IList<string> Sort { get; set; }

        /// <summary>
        /// Include the full content of the documents in the return.
        /// </summary>
        bool? IncludeDocs { get; set; }

        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        int? Limit { get; set; }

        /// <summary>
        /// Defines ranges for faceted numeric search fields.
        /// </summary>
        object Ranges { get; set; }

        /// <summary>
        /// List of field names for which counts should be produced.
        /// </summary>
        IList<string> Counts { get; set; }

        /// <summary>
        /// Field by which to group search matches.
        /// </summary>
        string GroupField { get; set; }

        /// <summary>
        /// Maximum group count. This field can only be used if group_field is specified.
        /// </summary>
        int? GroupLimit { get; set; }

        /// <summary>
        /// This field defines the order of the groups in a search using group_field.
        /// The default sort order is relevance.
        /// </summary>
        IList<string> GroupSort { get; set; }
    }
}