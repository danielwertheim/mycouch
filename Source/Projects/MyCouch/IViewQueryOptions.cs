using System.Collections.Generic;

namespace MyCouch
{
    public interface IViewQueryOptions : IEnumerable<KeyValuePair<string, string>>
    {
        string StartKey { get; set; }
        string EndKey { get; set; }
        int Skip { get; set; }
        int Limit { get; set; }
        bool Reduce { get; set; }
    }
}