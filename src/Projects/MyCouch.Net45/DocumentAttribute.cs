using System;
using MyCouch.Serialization.Meta;

namespace MyCouch
{
    /// <summary>
    /// Optional, can be used as a provider of meta-data
    /// about your documents on your entities.
    /// Is e.g. used by <see cref="IDocumentSerializationMetaProvider"/>
    /// to extract a custom value for the $doctype property
    /// injected upon serialization of documents for persisting documents.
    /// </summary>
#if !NETFX_CORE
    [Serializable]
#endif
    public class DocumentAttribute : Attribute
    {
        /// <summary>
        /// If specified, will e.g. override the default
        /// generated $doctype value.
        /// </summary>
        public string DocType { get; set; }
    }
}