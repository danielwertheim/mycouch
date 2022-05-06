using System.Collections.Generic;

namespace MyCouch
{
    public interface IFindParameters
    {
        /// <summary>
        /// JSON object describing criteria used to select documents.
        /// </summary>
        string SelectorExpression { get; set; }

        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        int? Limit { get; set; }

        /// <summary>
        /// Skip the first n results, where n is the value specified.
        /// </summary>
        int? Skip { get; set; }

        /// <summary>
        /// List of fields with sort directions to specify sorting of results.
        /// </summary>
        IList<SortableField> Sort { get; set; }

        /// <summary>
        /// The list of fields of the documents to be returned.
        /// </summary>
        IList<string> Fields { get; set; }

        /// <summary>
        /// Read quorum needed for the result.
        /// </summary>
        int? ReadQuorum { get; set; }

        /// <summary>
        ///  Include conflicted documents if true. Intended use is to easily find conflicted documents, without an index or view.
        /// </summary>
        public bool? Conflicts { get; set; }

        /// <summary>
        /// Whether or not the view results should be returned from a “stable” set of shards.
        /// </summary>
        public bool? Stable { get; set; }

        /// <summary>
        /// Whether to update the index prior to returning the result.
        /// </summary>
        public bool? Update { get; set; }

        /// <summary>
        /// Instruct a query to use a specific index. Specified either as "design_document"
        /// </summary>
        public string UseIndex { get; set; }

        /// <summary>
        /// A string that enables you to specify which page of results you require. Used for paging through result sets. Every query returns an opaque string under the bookmark key that can then be passed back in a query to get the next page of results. If any part of the selector query changes between requests, the results are undefined.
        /// </summary>
        public string Bookmark { get; set; }
    }
}
