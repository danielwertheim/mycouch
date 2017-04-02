namespace MyCouch
{
    /// <summary>
    /// Depicts the type of index. The only supported value is Json.
    /// Full text and geospatial indexes is to be supported in the future.
    /// </summary>
    public enum IndexType
    {
        Json = 0
    }

    internal static class IndexTypeEnumExtensions
    {
        public static string AsString(this IndexType indexType)
        {
            return indexType.ToString().ToLowerInvariant();
        }
    }
}
