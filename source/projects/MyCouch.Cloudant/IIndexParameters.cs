using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCouch.Cloudant
{
    /// <summary>
    /// The different parameters that can be specified while creating/deleting an index
    /// </summary>
    public interface IIndexParameters
    {
        /// <summary>
        /// The design document to which an index belongs
        /// </summary>
        string DesignDocument { get; set; }
        /// <summary>
        /// The type of index
        /// </summary>
        IndexType? Type { get; set; }
        /// <summary>
        /// The name of the index
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Index fields 
        /// </summary>
        IList<SortableField> Fields { get; set; }
    }
}
