using EnsureThat;
using System.Linq;

namespace MyCouch
{
    public class IndexParametersConfigurator
    {
        protected readonly IIndexParameters Parameters;

        public IndexParametersConfigurator(IIndexParameters parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// The design document to which an index belongs
        /// </summary>
        /// <param name="value">DEsign document name</param>
        /// <returns></returns>
        public virtual IndexParametersConfigurator DesignDocument(string value)
        {
            Ensure.String.IsNotNullOrWhiteSpace(value, nameof(value));

            Parameters.DesignDocument = value;

            return this;
        }
        /// <summary>
        /// The type of index
        /// </summary>
        /// <param name="type">Index type</param>
        /// <returns></returns>
        public virtual IndexParametersConfigurator Type(IndexType type)
        {
            Parameters.Type = type;

            return this;
        }

        /// <summary>
        /// The name of the index
        /// </summary>
        /// <param name="value">Name of the index</param>
        /// <returns></returns>
        public virtual IndexParametersConfigurator Name(string value)
        {
            Ensure.String.IsNotNullOrWhiteSpace(value, nameof(value));

            Parameters.Name = value;

            return this;
        }

        /// <summary>
        /// Index fields
        /// </summary>
        /// <param name="fields">Fields</param>
        /// <returns></returns>
        public virtual IndexParametersConfigurator Fields(params SortableField[] fields)
        {
            EnsureArg.HasItems(fields, nameof(fields));

            Parameters.Fields = fields.ToList();

            return this;
        }

        /// <summary>
        /// Index fields
        /// </summary>
        /// <param name="fields">Fields</param>
        /// <returns></returns>
        public virtual IndexParametersConfigurator Fields(params string[] fields)
        {
            EnsureArg.HasItems(fields, nameof(fields));

            Parameters.Fields = fields.Select(f => new SortableField(f)).ToList();

            return this;
        }
    }
}
