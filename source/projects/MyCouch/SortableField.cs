using EnsureThat;
using MyCouch.Serialization.Converters;
using Newtonsoft.Json;

namespace MyCouch
{
    [JsonConverter(typeof(SortableFieldConverter))]
    public class SortableField
    {
        public string Name { get; private set; }
        public SortDirection SortDirection { get; private set; }

        public SortableField(string name, SortDirection sortDirection = SortDirection.Asc)
        {
            EnsureArg.IsNotNullOrWhiteSpace(name, nameof(name));

            Name = name;
            SortDirection = sortDirection;
        }
    }
}
