using System;

namespace MyCouch
{
    public enum SortDirection
    {
        Asc,
        Desc
    }

    public static class SortDirectionEnumExtensions
    {
        public static string AsString(this SortDirection sortDirection)
        {
            return sortDirection.ToString().ToLowerInvariant();
        }

        public static SortDirection AsSortDirection(this string sortDirectionString)
        {
            return (SortDirection)Enum.Parse(typeof(SortDirection), sortDirectionString, true);
        }
    }
}
