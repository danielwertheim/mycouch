using System.Net.Http;

namespace MyCouch
{
    internal static class ResponseExtensions
    {
        internal static bool ContentShouldHaveIdAndRev(this IResponse response)
        {
            return
                response.RequestMethod == HttpMethod.Post ||
                response.RequestMethod == HttpMethod.Put ||
                response.RequestMethod == HttpMethod.Delete;
        }
    }
}