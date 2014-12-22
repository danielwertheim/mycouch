
using System.Collections.Generic;
namespace MyCouch
{
    /// <summary>
    /// The different common show parameters that can be specified
    /// when performing a query against a show function.
    /// </summary>
    public interface IShowParameters
    {
        /// <summary>
        /// Identitfies the Show function that this request will be
        /// performed against.
        /// </summary>
        ShowIdentity ShowIdentity { get; }
        /// <summary>
        /// The id of the document to which the show function will be applied.
        /// </summary>
        string Id { get; set; }
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
