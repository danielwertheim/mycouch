using System.Collections.Generic;

namespace MyCouch.Cloudant.Querying
{
    public class IndexParameters : IIndexParameters
    {
        public string DesignDocument { get; set; }
        public IndexType? Type { get; set; }
        public string Name { get; set; }
        public IList<IndexField> Fields { get; set; }
        public IndexParameters()
        {
            Fields = new List<IndexField>();
        }
    }
}
