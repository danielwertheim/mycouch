
using MyCouch.Cloudant.Serialization.Converters;
using Newtonsoft.Json;
namespace MyCouch.Cloudant
{
    [JsonConverter(typeof(IndexFieldConverter))]
    public class IndexField
    {
        public string Name { get; private set; }
        public SortDirection SortDirection { get; private set; }

        public IndexField(string name, SortDirection sortDirection = SortDirection.Asc)
        {
            Name = name;
            SortDirection = sortDirection;
        }
    }
}
