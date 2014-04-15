using MyCouch.Requests;

namespace MyCouch.Querying
{
    internal static class QueryExtensions
    {
        internal static QueryViewRequest ToRequest(this Query query)
        {
            return new QueryViewRequest(query);
        }
    }
}