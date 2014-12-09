
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
        string ViewName { get; set; }
        /// <summary>
        /// Return only documents that match the specified key.
        /// </summary>
        object Key { get; set; }

        //Plan to add other view query options in simimar pattern as above.

        IDictionary<string, object> AdditionalQueryParameters { get; set; }
        bool HasAdditionalQueryParameters { get; }
    }
}
