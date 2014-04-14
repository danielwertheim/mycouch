using System.Collections.Generic;

namespace MyCouch
{
    public enum Stale
    {
        Ok,
        UpdateAfter
    }

    public static class StaleEnumExtensions
    {
        private static readonly Dictionary<Stale, string> Mappings;

        static StaleEnumExtensions()
        {
            Mappings = new Dictionary<Stale, string> {
                { Stale.Ok, "ok" },
                { Stale.UpdateAfter, "update_after" }
            };
        }

        public static string AsString(this Stale stale)
        {
            return Mappings[stale];
        }
    }
}