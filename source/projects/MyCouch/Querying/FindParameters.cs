using System.Collections.Generic;

namespace MyCouch.Querying
{
    public class FindParameters : IFindParameters
    {
        public string SelectorExpression { get; set; }

        public int? Limit { get; set; }

        public int? Skip { get; set; }

        public IList<SortableField> Sort { get; set; }

        public IList<string> Fields { get; set; }

        public int? ReadQuorum { get; set; }

        public bool? Conflicts { get; set; }

        public bool? Stable { get; set; }

        public bool? Update { get; set; }

        public string UseIndex { get; set; }

        public string Bookmark { get; set; }

        public FindParameters()
        {
            Sort = new List<SortableField>();
            Fields = new List<string>();
        }
    }
}
