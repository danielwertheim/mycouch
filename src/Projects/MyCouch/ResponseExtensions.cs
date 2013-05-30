using System.Net.Http;

namespace MyCouch
{
    internal static class ResponseExtensions
    {
        internal static bool ContentShouldHaveIdAndRev(this HttpResponseMessage response)
        {
            return
                response.RequestMessage.Method == HttpMethod.Post ||
                response.RequestMessage.Method == HttpMethod.Put ||
                response.RequestMessage.Method == HttpMethod.Delete;
        }
    }
}