
using MyCouch.Cloudant.Serialization.Converters;
using Newtonsoft.Json;
namespace MyCouch.Cloudant
{
    [JsonConverter(typeof(SortableFieldConverter))]
    public class SortableField
    {
        public string Name { get; private set; }
        public SortDirection SortDirection { get; private set; }

        public SortableField(string name, SortDirection sortDirection = SortDirection.Asc)
        {
            Name = name;
            SortDirection = sortDirection;
        }
    }
}
