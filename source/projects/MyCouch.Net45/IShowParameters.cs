
using System.Collections.Generic;
namespace MyCouch
{
    /// <summary>
    /// The different common parameters that can be specified
    /// when performing a query against a show function.
    /// </summary>
    public interface IShowParameters
    {
        /// <summary>
        /// Identitfies the show function that will be used for transformation.
        /// </summary>
        ShowIdentity ShowIdentity { get; }
        /// <summary>
        /// Used to set custom accept header values.
        /// Applicable e.g. when specifying a <see cref="ShowIdentity"/>
        /// that returns e.g. HTML.
        /// </summary>
        string[] Accepts { get; set; }
        /// <summary>
        /// Indicates if any <see cref="Accepts"/> has been specified.
        /// </summary>
        bool HasAccepts { get; }
        /// <summary>
        /// The document id ofthe document to be transformed
        /// </summary>
        string DocId { get; set; }
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
