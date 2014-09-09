
using System.Collections.Generic;
namespace MyCouch.Cloudant
{
    /// <summary>
    /// Depicts the type of index. The only supported value is Json, Full text & geispatial indexes to be supported
    /// in future
    /// </summary>
    public enum IndexType
    {
        /// <summary>
        /// Json
        /// </summary>
        Json,
    }

    public static class IndexTypeEnumExtensions
    {
        private static readonly Dictionary<IndexType, string> Mappings;

        static IndexTypeEnumExtensions()
        {
            Mappings = new Dictionary<IndexType, string> {
                { IndexType.Json, "json" }
            };
        }

        public static string AsString(this IndexType IndexType)
        {
            return Mappings[IndexType];
        }
    }
}
