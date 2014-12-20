
using System.Collections.Generic;
namespace MyCouch.Cloudant
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
    }
}
