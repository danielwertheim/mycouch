using MyCouch.Net;
using System.Net.Http;

namespace MyCouch.Cloudant.HttpRequestFactories
{
    public class GetAllIndexesHttpRequestFactory
    {
        public virtual HttpRequest Create()
        {
            return new HttpRequest(HttpMethod.Get, GenerateRelativeUrl());
        }

        protected virtual string GenerateRelativeUrl()
        {
            return "/_index";
        }
    }
}
