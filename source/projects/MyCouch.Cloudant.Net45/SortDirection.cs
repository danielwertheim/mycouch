
using System.Collections.Generic;
namespace MyCouch.Cloudant
{
    public enum SortDirection
    {
        Asc,
        Desc
    }

    public static class SortDirectionEnumExtensions
    {
        private static readonly Dictionary<SortDirection, string> Mappings;

        static SortDirectionEnumExtensions()
        {
            Mappings = new Dictionary<SortDirection, string> {
                { SortDirection.Asc, "asc" },
                { SortDirection.Desc, "desc" }
            };
        }

        public static string AsString(this SortDirection sortDirection)
        {
            return Mappings[sortDirection];
        }
    }
}
