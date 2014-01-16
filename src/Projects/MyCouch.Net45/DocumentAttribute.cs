using System;
using MyCouch.Serialization.Meta;

namespace MyCouch
{
    /// <summary>
    /// Optional, can be used as a provider of meta-data
    /// about your documents or your entities.
    /// Is e.g. used by <see cref="IDocumentSerializationMetaProvider"/>
    /// to extract custom values for e.g. the $doctype property
    /// injected upon serialization of documents for persisting documents.
    /// </summary>
#if !NETFX_CORE
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Class)]
    public class DocumentAttribute : Attribute
    {
        /// <summary>
        /// Used to override default $doctype.
        /// </summary>
        public string DocType { get; set; }

        /// <summary>
        /// Used to generate $docns.
        /// </summary>
        public string DocNamespace { get; set; }

        /// <summary>
        /// Used to generate $docver.
        /// </summary>
        public string DocVersion { get; set; }
    }
}