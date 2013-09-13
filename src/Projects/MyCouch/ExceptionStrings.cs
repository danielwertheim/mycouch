namespace MyCouch
{
    public static class ExceptionStrings
    {
        public static string BasicHttpClientConnectionUriIsMissingDb
        {
            get { return "No database seems to be specified."; }
        }

        public static string QueryResponseRowsDeserializerNeedsJsonArray
        {
            get { return "When deserializing query response rows, the JSON-reader should point to the Start of an array."; }
        }
    }
}
