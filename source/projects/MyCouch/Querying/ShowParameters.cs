using EnsureThat;
using System.Collections.Generic;
using System.Linq;

namespace MyCouch.Querying
{
    public class ShowParameters : IShowParameters
    {

        /// <summary>
        /// Identitfies the show function that will be used for transformation.
        /// </summary>
        public ShowIdentity ShowIdentity { get; private set; }

        /// <summary>
        /// Used to set custom accept header values.
        /// Applicable e.g. when specifying a <see cref="ShowIdentity"/>
        /// that returns e.g. HTML.
        /// </summary>
        public string[] Accepts { get; set; }

        /// <summary>
        /// Indicates if any <see cref="Accepts"/> has been specified.
        /// </summary>
        public bool HasAccepts
        {
            get { return Accepts != null && Accepts.Any(); }
        }

        /// <summary>
        /// The document id ofthe document to be transformed
        /// </summary>
        public string DocId { get; set; }

        /// <summary>
        /// Additional custom query string parameters.
        /// </summary>
        public IDictionary<string, object> CustomQueryParameters { get; set; }

        /// <summary>
        /// Indicates if there are any <see cref="CustomQueryParameters"/> or not.
        /// </summary>
        public bool HasCustomQueryParameters
        {
            get { return CustomQueryParameters != null && CustomQueryParameters.Any(); }
        }

        public ShowParameters(ShowIdentity showIdentity)
        {
            Ensure.That(showIdentity, "showIdentity").IsNotNull();

            ShowIdentity = showIdentity;
        }
    }
}
